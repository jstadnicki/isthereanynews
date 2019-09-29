using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

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

            var connectionString = config.GetConnectionString("emulator");

            var account = CloudStorageAccount.Parse(connectionString);
            var serviceClient = account.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference("rss");
            await container.CreateIfNotExistsAsync();
            var blob = container.GetBlockBlobReference($"{channelToDownload.Id}/{DateTime.UtcNow.ToString("yyyyMMddhhmmss_mmm")}");
            await blob.UploadTextAsync(channelString);

        }
    }
}
