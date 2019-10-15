using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Itan.Functions.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Itan.Functions.Workers
{
    public class Function4Worker
    {
        private ILogger logger;
        private readonly string functionAppDirectory;

        public Function4Worker(ILogger logger, string functionAppDirectory)
        {
            this.logger = logger;
            this.functionAppDirectory = functionAppDirectory;
        }

        public async Task RunAsync(ChannelUpdate message)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(this.functionAppDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            var sqlConnectionString = config.GetConnectionString("sql-itan-writer");
            var query = "UPDATE Channels SET Title = @title, Description = @description, ModifiedOn = @modified WHERE Id = @id";
            var queryData = new
            {
                message.Title,
                message.Description,
                modified = DateTime.UtcNow,
                message.Id
            };
            try
            {
                using (var sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    await sqlConnection.ExecuteAsync(query, queryData);
                }
            }
            catch (Exception e)
            {
                this.logger.LogCritical(e.ToString());
            }
        }
    }
}