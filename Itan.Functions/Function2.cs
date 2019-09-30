using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;

namespace Itan.Functions
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static async Task Run(
            [QueueTrigger("ChannelToDownload", Connection = "emulator")]string myQueueItem, 
            ExecutionContext context,
            ILogger log)
        {
            var channelToDownload = Newtonsoft.Json.JsonConvert.DeserializeObject<ChannelToDownload>(myQueueItem);
            var client = HttpClientFactory.Create();
            var channelString = await client.GetStringAsync(channelToDownload.Url);

            var config = new ConfigurationBuilder() 
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var emulatorConnectionString = config.GetConnectionString("emulator");

            var account = CloudStorageAccount.Parse(emulatorConnectionString);
            var serviceClient = account.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference("rss");
            await container.CreateIfNotExistsAsync();
            var channelDownloadPath = $"raw/{channelToDownload.Id}/{DateTime.UtcNow.ToString("yyyyMMddhhmmss_mmm")}.xml";
            var blob = container.GetBlockBlobReference(channelDownloadPath);
            await blob.UploadTextAsync(channelString);

            var query = "INSERT INTO ChannelDownloads (Id, ChannelId, Path, CreatedOn) VALUES (@id, @channelId, @path, @createdOn)";
            var data = new
            {
                id = Guid.NewGuid(), 
                channelId = channelToDownload.Id, 
                path = channelDownloadPath,
                createdOn = DateTime.UtcNow
            };

            var sqlConnectionString = config.GetConnectionString("sql-itan-writer");

            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(query, data);
            }
        }
    }
}
