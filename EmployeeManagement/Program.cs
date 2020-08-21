using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace EmployeeManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

             .ConfigureLogging((hostingContext, loggingBuilder) =>
             {
                 loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                 loggingBuilder.AddConsole();
                 loggingBuilder.AddDebug();
                 loggingBuilder.AddEventSourceLogger();
                 //enable nlog thirt part error logger to save error details.
                 loggingBuilder.AddNLog();                 
             })
            
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
