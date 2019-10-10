using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Itan.Functions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace Itan.Functions.Workers
{
    public class Function1Worker
    {
        private readonly ILogger logger;
        private readonly string functionAppDirectory;
        private readonly IAsyncCollector<ChannelToDownload> messagesCollector;

        public Function1Worker(
            ILogger logger,
            string functionAppDirectory,
            IAsyncCollector<ChannelToDownload> messagesCollector)
        {
            this.logger = logger;
            this.functionAppDirectory = functionAppDirectory;
            this.messagesCollector = messagesCollector;
        }

        public async Task Run()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(this.functionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = config.GetConnectionString("sql-itan-reader");
            List<ChannelToDownload> listOfChannelsToDownload;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var query = "SELECT c.Id, c.Url FROM Channels c";
                var queryResult = await sqlConnection.QueryAsync<ChannelToDownload>(query);
                listOfChannelsToDownload = queryResult.ToList();
            }

            foreach (var channelToDownload in listOfChannelsToDownload)
            {
                await this.messagesCollector.AddAsync(channelToDownload);
            }
        }
    }
}