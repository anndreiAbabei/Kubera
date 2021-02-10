using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Kubera.App.Data;
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
using System.Threading.Tasks;
using System.Threading;
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

namespace Kubera.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
            //.AddVersionedApiExplorer(options =>
            //{
            //    options.GroupNameFormat = "VVV";
            //    options.SubstituteApiVersionInUrl = true;
            //})
            .AddSingleton<FluentValidationSchemaProcessor>()
            .AddSwaggerDocument((settings, sp) => GenerateSwaggerDocument(settings, sp, "v1", "1"));

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true)
               .AddControllers(options =>
               {
                   options.EnableEndpointRouting = false;

                   options.ReturnHttpNotAcceptable = true;

                   options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest));
                   options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized)); // Even if Axway is returning this then we want it visible in the OpenAPI/Swagger doc (TODO: Will Axway return a ProblemDetails object??!?)
                   options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status403Forbidden));
                   options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable)); // This defaults payload to ProblemDetails which is wrong. So Schema is later removed from Swagger.json via an OperationProcessor
                   options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity));
                   options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), StatusCodes.Status500InternalServerError));
               })
               .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<ApplicationDom>())
               .AddJsonOptions(options => options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase);

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddMediatR(typeof(ApplicationDom).GetTypeInfo().Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            ConfigureDI(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
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
            {
                app.UseSpaStaticFiles();
            }
            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            MigrateAndSeed(app).ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        private void ConfigureDb(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        private void ConfigureDI(IServiceCollection services)
        {
            var settings = Configuration.GetSection("AppSettings").Get<AppSettings>();

            services.AddHttpContextAccessor();
            services.AddMemoryCache();

            services.AddSingleton<IAppSettings, AppSettings>(i => settings);
            services.AddSingleton(i => settings?.CacheOptions ?? CacheOptions.Default);
            services.AddScoped<IUserIdAccesor, HttpUserIdAccesor>();
            services.AddScoped<ICacheService, CacheService>();

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

        private static async ValueTask MigrateAndSeed(IApplicationBuilder app, CancellationToken cancellationToken = default)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            var seeder = serviceScope.ServiceProvider.GetService<ISeeder>();

            await context.Database.MigrateAsync(cancellationToken)
                .ConfigureAwait(false);

            await seeder.Seed(cancellationToken)
                .ConfigureAwait(false);
        }

        private void GenerateSwaggerDocument(AspNetCoreOpenApiDocumentGeneratorSettings settings, IServiceProvider sp, string name, string group)
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
                doc.Info.Title = "Kubera API";
                doc.Info.Description = "The Kubera API used for Kubera Angular UI";
                doc.Info.TermsOfService = "None";
                doc.Info.Contact = new OpenApiContact
                {
                    Name = "Kubera",
                    Email = ""
                };
            };
        }
    }
}
