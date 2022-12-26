using System;
using Autofac.Extensions.DependencyInjection;
using Azure.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Itan.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost
                .CreateDefaultBuilder(args)
                .ConfigureServices(x => x.AddAutofac())
                .ConfigureAppConfiguration((x, config) =>
                {
                    config.AddJsonFile("appsettings.json");
                    config.AddAzureKeyVault(new Uri("https://itan-key-vault.vault.azure.net"),
                        new DefaultAzureCredential());
                    config.AddJsonFile("local.settings.json", true);
                })
                .UseStartup<Startup>();
        }
    }
}