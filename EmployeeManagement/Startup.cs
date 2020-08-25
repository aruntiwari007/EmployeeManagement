using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace EmployeeManagement
{    
    public class Startup
    {
        private IConfiguration _confiq;
        public Startup(IConfiguration confiq)
        {
            _confiq = confiq;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(
                Options => Options.UseSqlServer(_confiq.GetConnectionString("EmployeeDbConnection")));
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                  .RequireAuthenticatedUser()
                                  .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.EnableEndpointRouting = false;
            }).AddXmlSerializerFormatters(); // add for giv the response in xaml format
          
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 3;
            }).AddEntityFrameworkStores<AppDbContext>();

            services.Configure<IdentityOptions>(options =>
               {
                   options.Password.RequiredLength = 10;//code for override the exsting validation of password                  
               });


        }      
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //DeveloperExceptionPageOptions _dev = new DeveloperExceptionPageOptions()
                //{
                //    SourceCodeLineCount = 20
                //};
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            //DefaultFilesOptions _defaultFilesOptions = new DefaultFilesOptions();
            //_defaultFilesOptions.DefaultFileNames.Clear();
            //_defaultFilesOptions.DefaultFileNames.Add("test.html");

            //app.UseDefaultFiles(_defaultFilesOptions); // alternate we can use filesrveroptions
           //app.UseRouting();
          app.UseStaticFiles();
            // app.UseMvcWithDefaultRoute();
            //  app.UseFileServer();
            app.UseAuthentication();
            app.UseMvc(configureRoutes => { configureRoutes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            //app.Run(async (context) =>
            //{

            //    await context.Response.WriteAsync("hosting enviroment is " + env.EnvironmentName);
            //});


            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync(_confiq["mykey"]);
            //    });
            //});
        }
    }
}
