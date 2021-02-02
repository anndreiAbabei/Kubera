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
using AutoMapper;
using Kubera.App.Mapper;
using System;

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
            });
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

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
        }

        private void ConfigureDb(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        private static void ConfigureDI(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IUserIdAccesor, HttpUserIdAccesor>();
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
    }
}
