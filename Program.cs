using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySaleApp.Admin.UI
{
    public class Program
    {
        private static string env = "";

        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            env = config.GetSection("Env").Value;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
            var url = $"http://0.0.0.0:{port}";

            return Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((a, config) =>
               {
                   config.AddJsonFile("appsettings.json");
                   config.AddJsonFile($"appsettings.{env}.json");
                   //config.AddJsonFile($"appsettings.prod.json");
               }
            )
              .ConfigureWebHostDefaults(webBuilder =>
              {
                  webBuilder.UseStartup<Startup>().UseUrls(url);
              });
        }
    }
}