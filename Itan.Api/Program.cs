using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

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
            var astp = new AzureServiceTokenProvider();
            var kvc = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(astp.KeyVaultTokenCallback));

            return WebHost
                .CreateDefaultBuilder(args)
                .ConfigureServices(x => x.AddAutofac())
                .ConfigureAppConfiguration((x, config) =>
                {
                    config.AddJsonFile("appsettings.json");
                    config.AddAzureKeyVault("https://itan-key-vault.vault.azure.net", 
                        kvc,
                        new DefaultKeyVaultSecretManager());
                })
                .UseStartup<Startup>();
        }
    }
}