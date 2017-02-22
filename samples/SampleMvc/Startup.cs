using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampleMvc.Models;
using AspNetCore.Identity.Marten;
using SampleMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace SampleMvc
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if(env.IsDevelopment())
                builder.AddUserSecrets();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc(options => {
                options.SslPort = 44317;
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddApplicationDocumentStore(Configuration);
            services.AddApplicationDocumentSession();
            services.AddApplicationSystemClock();

            services.AddApplicationIdentity()
                .AddMartenStores<ApplicationUser, Guid>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IEmailSender, LoggingAuthMessageSender>();
            services.AddSingleton<ISmsSender, LoggingAuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            bool fbEnabled = bool.TryParse(Configuration["Authentication:Facebook:Enabled"], out fbEnabled)? fbEnabled : false;

            if (fbEnabled)
                app.UseFacebookAuthentication(new FacebookOptions()
                {
                    AppId = Configuration["Authentication:Facebook:AppId"],
                    AppSecret = Configuration["Authentication:Facebook:AppSecret"]
                });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
