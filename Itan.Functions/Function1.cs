using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Itan.Functions
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task Run(
            [TimerTrigger("0 * */1 * * *")]TimerInfo myTimer,
            ExecutionContext context,
            ILogger log,
            [Queue("ChannelToDownload",Connection = "emulator")]IAsyncCollector<ChannelToDownload> messagesCollector
            )
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var query = "SELECT c.Id, c.Url FROM Channels c";

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = config.GetConnectionString("sql-itan-reader");
            List<ChannelToDownload> listOfChannelsToDownload;
            
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var queryResult = await sqlConnection.QueryAsync<ChannelToDownload>(query);
                listOfChannelsToDownload = queryResult.ToList();
            }

            foreach (var channelToDownload in listOfChannelsToDownload)
            {
                await messagesCollector.AddAsync(channelToDownload);
            }
        }
    }

    public class ChannelToDownload
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
    }
}