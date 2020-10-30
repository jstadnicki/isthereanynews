using System.IO;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace Itan.Database
{
    internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EntityFrameworkContext>
    {
        public EntityFrameworkContext CreateDbContext(string[] args)
        {
            var astp = new AzureServiceTokenProvider();
            var kvc = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(astp.KeyVaultTokenCallback));

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddAzureKeyVault("https://itan-key-vault.vault.azure.net", kvc, new DefaultKeyVaultSecretManager())
                .Build();

            var connectionString = configuration.GetValue<string>("SqlAdminConnectionString");

            var builder = new DbContextOptionsBuilder<EntityFrameworkContext>();
            // var connectionString =
            //     kvc.GetSecretAsync("https://itan-key-vault.vault.azure.net/", "SqlAdminConnectionString")
            //        .GetAwaiter()
            //        .GetResult()
            //        .Value;
            builder.UseSqlServer(connectionString);
            return new EntityFrameworkContext(builder.Options);
        }
    }
}