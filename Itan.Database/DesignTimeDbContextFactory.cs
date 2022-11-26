using System;
using System.IO;
using Azure.Identity;
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
                .AddAzureKeyVault(new Uri("https://itan-key-vault.vault.azure.net"), new DefaultAzureCredential())
                .Build();

            var builder = new DbContextOptionsBuilder<EntityFrameworkContext>();
            var connectionString = configuration["SqlAdminConnectionString"];

            Console.WriteLine(connectionString);

            builder.UseSqlServer(connectionString);
            return new EntityFrameworkContext(builder.Options);
        }
    }
}