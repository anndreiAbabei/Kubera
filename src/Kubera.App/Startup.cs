using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Kubera.Data.Entities;
using Kubera.Data;
using Microsoft.AspNetCore.Mvc;
using Kubera.General.Services;
using Kubera.App.Infrastructure.Services;
using Kubera.Business.Repository;
using Kubera.Data.Store;
using Kubera.General;
using Kubera.Business.Seed;
using FluentValidation.AspNetCore;
using Kubera.App.Mapper;
using Kubera.Application;
using System;
using Kubera.Business.Services;
using ZymLabs.NSwag.FluentValidation;
using NSwag.Generation.AspNetCore;
using NSwag;
using System.Text.Json;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using MediatR;
using System.Reflection;
using Kubera.App.Infrastructure.Behaviours;
using Kubera.Application.Services;
using Kubera.App.Infrastructure;
using Kubera.App.Static;
using Kubera.Business.Entities;
using Kubera.Data.Data;
using Kubera.General.Defaults;
using Kubera.App.Infrastructure.Environment;
using System.Linq;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using Kubera.App.Infrastructure.Mail;

namespace Kubera.App
{
    public class Startup
    {
        private const string _corsPolicy = "_KUBERA_API_CORS";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var settings = GetSettings();
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(b => ConfigureDb(b, settings));

            ConfigureAuthorisation(services, settings);
            ConfigureCors(services, settings);
            ConfigureApi(services);
            ConfigureClient(services);

            ConfigureDi(services, settings);

            services.AddHostedService<StartupSeedService>();
            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var settings = app.ApplicationServices.GetRequiredService<IAppSettings>();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseHsts();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
                app.UseSpaStaticFiles();
            
            app.UseRouting();

            if (settings.Cors != null)
                app.UseCors(_corsPolicy);

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHealthChecks("/health");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment()) 
                    spa.UseAngularCliServer("start");
            });
        }

        private IAppSettings GetSettings()
        {
            var settings = Configuration.GetSection(SettingKeys.AppSettings).Get<AppSettings>();

            settings.AlphaVantageApiKey = Configuration[SettingKeys.AlphaVantageApiKey];
            settings.DatabaseConnectionString = Configuration[SettingKeys.ConnectionStrKuberaDb];

            if(settings.Mail != null)
                settings.Mail.ApiKey = Configuration[SettingKeys.MailjetKey];

            return settings;
        }

        private static void ConfigureDb(DbContextOptionsBuilder builder, IAppSettings settings)
        {
            builder.UseSqlServer(settings.DatabaseConnectionString, options => options.EnableRetryOnFailure(settings.DatabaseRetries));
        }

        private void ConfigureAuthorisation(IServiceCollection services, IAppSettings settings)
        {
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                            .AddEntityFrameworkStores<ApplicationDbContext>();

            var identityBuilder = services.AddIdentityServer(options => ConfigureIdentityServer(options, settings))
                .AddAspNetIdentity<ApplicationUser>()
                .AddOperationalStore<ApplicationDbContext>()
                .AddIdentityResources()
                .AddApiResources()
                .AddClients();
            if (Configuration.GetAppEnvironment() == AppEnvironment.Developement)
                identityBuilder.AddDeveloperSigningCredential();
            else
            {
                var appCert = Program.GetCertificate(Configuration, "Application:Identity");

                if (appCert != null)
                    identityBuilder.AddSigningCredential(appCert);
            }

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddHttpClient<IEmailSender, MailjetMailSender>();
        }

        private void ConfigureIdentityServer(IdentityServerOptions options, IAppSettings settings)
        {
            if (settings.Autorisation == null)
                return;

            if(!string.IsNullOrEmpty(settings.Autorisation.ValidIssuer))
                options.IssuerUri = settings.Autorisation.ValidIssuer;
        }

        private static void ConfigureCors(IServiceCollection services, IAppSettings settings)
        {
            const string all = "*";
            const string separator = ",";

            if (settings.Cors == null)
                return;

            services.AddCors(options =>
            {
                options.AddPolicy(_corsPolicy, builder =>
                {
                    builder.WithOrigins(settings.Cors.Origins.ToArray());

                    if (settings.Cors.AllowHeaders == all)
                        builder.AllowAnyHeader();
                    else
                        builder.WithHeaders(settings.Cors.AllowHeaders.Split(separator));

                    if (settings.Cors.AllowMethods == all)
                        builder.AllowAnyMethod();
                    else
                        builder.WithMethods(settings.Cors.AllowMethods.Split(separator));

                    if (settings.Cors.AllowedCredentials)
                        builder.AllowCredentials();
                });
            });
        }

        private static void ConfigureApi(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
            })
            .AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "VVV";
                options.SubstituteApiVersionInUrl = true;
            })
            .AddSingleton<FluentValidationSchemaProcessor>()
            .AddSwaggerDocument((settings, sp) => GenerateSwaggerDocument(settings, sp, "v1", "1"));

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true)
               .AddControllers(options =>
               {
                   options.EnableEndpointRouting = false;

                   options.ReturnHttpNotAcceptable = true;

                   options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest));
                   options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));
                   options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status403Forbidden));
                   options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                   options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity));
                   options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), StatusCodes.Status500InternalServerError));
               })
               .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<ApplicationDom>())
               .AddJsonOptions(options => options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase);
        }

        private static void ConfigureClient(IServiceCollection services)
        {
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/dist");
        }

        private static void ConfigureDi(IServiceCollection services, IAppSettings settings)
        {
            services.AddMediatR(typeof(ApplicationDom).GetTypeInfo().Assembly)
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddHttpContextAccessor();

            services.AddHttpClient<IForexService, AlphaVantageService>();
            
            services.AddSingleton(_ => settings);
            services.AddSingleton(_ => settings.CacheOptions ?? CacheOptions.Default);
            services.AddScoped<IDefaultEntities, DefaultEntities>();
            services.AddScoped<IDefaults, DefaultEntities>();
            services.AddScoped<IUserIdAccesor, HttpUserIdAccesor>();
            services.AddTransient<ICacheService, CacheService>(); 
            services.AddTransient<IUserCacheService, UserCacheService>();

            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IAssetStore, AssetStore>();
            services.AddScoped<ICurrencyStore, CurrencyStore>();
            services.AddScoped<IGroupStore, GroupStore>();
            services.AddScoped<ITransactionStore, TransactionStore>();
            services.AddScoped<IUserStore, UserStore>();

            services.AddScoped<ISeeder, AppSeeder>();

            services.AddAutoMapper(typeof(AppMapper).Assembly);
        }

        private static void GenerateSwaggerDocument(AspNetCoreOpenApiDocumentGeneratorSettings settings, IServiceProvider sp, string name, string group)
        {
            var fluentValidationSchemaProcessor = sp.GetRequiredService<FluentValidationSchemaProcessor>();
            settings.SchemaProcessors.Add(fluentValidationSchemaProcessor);

            settings.DocumentName = name;
            settings.ApiGroupNames = new[] { group };

            settings.FlattenInheritanceHierarchy = true;
            settings.GenerateEnumMappingDescription = true;

            settings.PostProcess = doc =>
            {
                doc.Schemes = new[] { OpenApiSchema.Https };
                doc.Info.Version = name;
                doc.Info.Title = $"{Resources.ApiName} API";
                doc.Info.Description = $"The {Resources.ApiName} API used for {Resources.AppName} UI App";
                doc.Info.TermsOfService = "MIT";
                doc.Info.Contact = new OpenApiContact
                {
                    Name = Resources.AppName,
                    Email = ""
                };
            };
        }
    }
}
