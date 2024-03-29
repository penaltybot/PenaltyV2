﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PenaltyV2.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Identity.UI.Services;
using PenaltyV2.Areas.Identity.Pages.Account;

namespace PenaltyV2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddTransient<IEmailSender, sendMail>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Database.GetConnectionString()
                    ));
            services.AddDefaultIdentity<IdentityUser>( Configuration => {
                Configuration.SignIn.RequireConfirmedEmail = false;
                Configuration.Password.RequireDigit = false;
                Configuration.Password.RequiredLength = 6;
                Configuration.Password.RequireLowercase = false;
                Configuration.Password.RequireNonAlphanumeric = false;
                Configuration.Password.RequireUppercase = false;
            })
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-PT"),
                SupportedCultures = new List<CultureInfo> { new CultureInfo("pt-PT") },
                SupportedUICultures = new List<CultureInfo> { new CultureInfo("pt-PT") }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
