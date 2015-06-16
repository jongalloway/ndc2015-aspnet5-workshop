﻿using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Diagnostics.Entity;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using UnicornStore.AspNet.Models.Identity;
using UnicornStore.AspNet.Models.UnicornStore;
using UnicornStore.Logging;
using UnicornStore.Models;

namespace UnicornStore
{
    public class Startup
    {
        private bool _useFacebookAuth;
        private bool _useGoogleAuth;

        public Startup(IHostingEnvironment env)
        {
            // Setup configuration sources.
            var configuration = new Configuration()
                .AddJsonFile("config.json")
                .AddJsonFile("secrets.json", optional: true)
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                configuration.AddUserSecrets();
            }

            configuration.AddEnvironmentVariables();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Application settings to the services container.
            services.Configure<AppSettings>(Configuration.GetSubKey("AppSettings"));

            // Add EF services to the services container.
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<UnicornStoreContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:UnicornStore"]))
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:UnicornStore"]));

            services.AddSingleton<CategoryCache>();

            // Add Identity services to the services container.
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // See comments in config.json for info on enabling Facebook auth
            var facebookId = Configuration.Get("Secrets:Facebook:AppId");
            var facebookSecret = Configuration.Get("Secrets:Facebook:AppSecret");
            if (!string.IsNullOrWhiteSpace(facebookId) && !string.IsNullOrWhiteSpace(facebookSecret))
            {
                _useFacebookAuth = true;
                services.ConfigureFacebookAuthentication(options =>
                {
                    options.AppId = facebookId;
                    options.AppSecret = facebookSecret;
                });
            }

            // See comments in config.json for info on enabling Google auth
            var googleId = Configuration.Get("Secrets:Google:ClientId");
            var googleSecret = Configuration.Get("Secrets:Google:ClientSecret");
            if (!string.IsNullOrWhiteSpace(googleId) && !string.IsNullOrWhiteSpace(googleSecret))
            {
                _useGoogleAuth = true;
                services.ConfigureGoogleAuthentication(options =>
                {
                    options.ClientId = googleId;
                    options.ClientSecret = googleSecret;
                });
            }

            // Add MVC services to the services container.
            services.AddMvc();

            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
        {
            // Configure the HTTP request pipeline.

            // Add the console logger.
            loggerfactory.AddConsole(minLevel: LogLevel.Warning);

            loggerfactory.AddProvider(new SqlLoggerProvider());

            // Add the following to the request pipeline only in development environment.
            if (env.IsEnvironment("Development"))
            {
                app.UseBrowserLink();
                app.UseErrorPage(ErrorPageOptions.ShowAll);
                app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);
                app.EnsureMigrationsApplied();
                app.EnsureSampleData();
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // sends the request to the following path or controller action.
                app.UseErrorHandler("/Home/Error");
            }

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add cookie-based authentication to the request pipeline.
            app.UseIdentity();

            if (_useFacebookAuth)
            {
                app.UseFacebookAuthentication();
            }

            if (_useGoogleAuth)
            {
                app.UseGoogleAuthentication();
            }

            app.EnsureRolesCreated();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });
        }
    }
}
