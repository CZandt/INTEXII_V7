using Identity.CustomPolicy;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.EntityFrameworkCore.Sqlite;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Identity.Models.Mummy;
using Microsoft.AspNetCore.Http;
using Microsoft.ML.OnnxRuntime;

namespace Identity
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
                // This lambda determines whether user conset for non - essential cookies is needed for a given context

                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;

            });

            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlite(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddDbContext<ebdbContext>(options => options.UseNpgsql(Configuration["ConnectionStrings:MummyConnection"]));
           
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
            services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(10));
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = ".AspNetCore.Identity.Application";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
            });


            services.Configure<IdentityOptions>(opts =>
            {
                opts.Lockout.AllowedForNewUsers = true;
                opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                opts.Lockout.MaxFailedAccessAttempts = 3;
                opts.Password.RequiredLength = 8;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireNonAlphanumeric = true;
            });

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });
             
            services.AddSingleton<InferenceSession>(
                new InferenceSession("wwwroot/head_directioon_model.onnx"));

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name:"filterpage",
                //    pattern: "{query}/Page{pageNum}",
                //    new { Controller = "Table", action = "Summary" });

                ////endpoints.MapControllerRoute(
                ////    name: "Paging",
                ////    pattern: "Page{pageNum}",
                ////    defaults: new { Controller = "Table", action = "Summary", pageNum = 1 });

                //endpoints.MapControllerRoute(
                //    name: "filter",
                //    pattern:"{query}",
                //    new { Controller = "Table", action = "Summary", pageNum = 1 });

                //endpoints.MapControllerRoute(
                //    name: "Paging",
                //    pattern: "Page{pageNum}",
                //    defaults: new { Controller = "Table", action = "Summary", pageNum = 1 });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
