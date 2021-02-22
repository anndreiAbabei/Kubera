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

namespace Kubera.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        private IConfiguration Configuration { get; }



        public void ConfigureServices(IServiceCollection services)
        {
#if DEBUG
            services.AddDatabaseDeveloperPageExceptionFilter();
#endif
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(ConfigureDb);

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddAuthentication()
                    .AddIdentityServerJwt();
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

            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/dist");

            services.AddMediatR(typeof(ApplicationDom).GetTypeInfo().Assembly)
                    .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>))
                    .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggingBehavior<,>));;

            services.AddHostedService<StartupSeedService>();

            ConfigureDi(services);
        }



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
                app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                    spa.UseAngularCliServer("start");
            });
        }



        private void ConfigureDb(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(Configuration[SettingKeys.ConnectionStrKuberaDb],
                                 options => options.EnableRetryOnFailure());
        }



        private void ConfigureDi(IServiceCollection services)
        {
            var settings = Configuration.GetSection(SettingKeys.AppSettings).Get<AppSettings>();
            settings.AlphaVantageApiKey = Configuration[SettingKeys.AlphaVantageApiKey];

            services.AddHttpContextAccessor();

            services.AddHttpClient<IForexService, AlphaVantageService>();

            services.AddSingleton<IAppSettings, AppSettings>(_ => settings);
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
            settings.ApiGroupNames = new[] {group};

            settings.FlattenInheritanceHierarchy = true;
            settings.GenerateEnumMappingDescription = true;

            settings.PostProcess = doc =>
            {
                doc.Schemes = new[] {OpenApiSchema.Https};
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
