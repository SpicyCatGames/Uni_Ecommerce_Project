using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodMarket.Data;
using Microsoft.EntityFrameworkCore;
using FoodMarket.Data.Repository;
using Microsoft.AspNetCore.Identity;
using FoodMarket.Data.FileManager;
using Microsoft.AspNetCore.Mvc;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Pomelo.EntityFrameworkCore.MySql.Internal;

namespace FoodMarket
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
            services.AddControllersWithViews();
            //https://code-maze.com/aspnetcore-response-caching/

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength= 6;
            })
                //.AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
            });

            var connectionString = Configuration["DefaultConnection"];
            var serverVersion = Configuration["MySQLServerVersion"];

            services.AddDbContext<AppDbContext>(opts => opts.UseMySql(connectionString, mysqlOptions =>
            {
                mysqlOptions.ServerVersion(serverVersion);
                mysqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                mysqlOptions.MinBatchSize(1);
                mysqlOptions.MaxBatchSize(42);
            }));
            services.AddTransient<IRepository, Repository>();
            services.AddTransient<IFileManager, FileManager>();
            // dotnet ef migrations add MigrationName
            // dotnet ef database update
            services.AddResponseCaching();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            

            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            //app.Run(async context =>
            //    await context.Response.WriteAsync("Hello World")
            //);
        }
    }
}
