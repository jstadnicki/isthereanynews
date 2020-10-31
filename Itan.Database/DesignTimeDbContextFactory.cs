using System;
using System.IO;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Itan.Database
{
    internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EntityFrameworkContext>
    {
        public EntityFrameworkContext CreateDbContext(string[] args)
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                // .AddAzureKeyVault("https://itan-key-vault.vault.azure.net", kvc, new DefaultKeyVaultSecretManager())
                .Build();

            var connectionString = Environment.GetEnvironmentVariable("SqlAdminConnectionString");

            System.Diagnostics.Debug.WriteLine("CS:"+connectionString);
            System.Diagnostics.Trace.WriteLine("CS:"+connectionString);
            Console.WriteLine("CS:"+connectionString);
            File.WriteAllText("log.txt", "CS:"+connectionString);

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