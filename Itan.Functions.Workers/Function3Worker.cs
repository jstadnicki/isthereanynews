using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Dapper;
using Itan.Functions.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Itan.Functions
{
    public class Function3Worker
    {
        private readonly ILogger logger;
        private readonly string functionAppDirectory;

        public Function3Worker(ILogger logger, string functionAppDirectory)
        {
            this.logger = logger;
            this.functionAppDirectory = functionAppDirectory;
        }

        public async Task RunAsync(Guid channelId, string blobName, Stream myBlob)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(this.functionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var emulatorConnectionString = config.GetConnectionString("emulator");
            var account = CloudStorageAccount.Parse(emulatorConnectionString);

            this.logger.LogInformation($"Got channel with id: {channelId} with name: {blobName} witn stream length: {myBlob.Length}");
            IEnumerable<FeedItem> feedItems;



            try
            {
                TextReader tr = new StreamReader(myBlob);
                var feedString = tr.ReadToEnd();
                var feed = FeedReader.ReadFromString(feedString);
                feedItems = feed.Items;
                var channelUpdate = new ChannelUpdate
                {
                    Title = feed.Title,
                    Description = feed.Description,
                    Id = channelId
                };

                var queueClient = account.CreateCloudQueueClient();
                var cloudQueue = queueClient.GetQueueReference(QueuesName.ChannelUpdate);
                await cloudQueue.CreateIfNotExistsAsync();
                var jsonMessage = JsonConvert.SerializeObject(channelUpdate);
                await cloudQueue.AddMessageAsync(new CloudQueueMessage(jsonMessage));
            }
            catch (Exception e)
            {
                this.logger.LogCritical($"Xml rss/raw/{channelId}/{blobName} is broken.Finishing and returning");
                this.logger.LogCritical(e.ToString());
                return;
            }



            var serviceClient = account.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference("rss");
            await container.CreateIfNotExistsAsync();

            foreach (var item in feedItems)
            {
                var itemJson = JsonConvert.SerializeObject(item);
                var itemJsonGuid = Guid.NewGuid();

                var itemUploadPath = $"items/{channelId}/{itemJsonGuid.ToString()}.json";
                var blob = container.GetBlockBlobReference(itemUploadPath);
                await blob.UploadTextAsync(itemJson);

                var query = "INSERT INTO News (Id, ChannelId, Title, CreatedOn, HashCode) VALUES (@id, @channelId, @title, @createdOn, @hashCode)";

                var ho = new
                {
                    title = item.Title.Trim(),
                    channelId
                }.GetHashCode();

                var data = new
                {
                    id = itemJsonGuid,
                    channelId = channelId,
                    title = item.Title.Trim(),
                    createdOn = DateTime.UtcNow,
                    hashCode = ho
                };

                var sqlConnectionString = config.GetConnectionString("sql-itan-writer");

                try
                {
                    using (var sqlConnection = new SqlConnection(sqlConnectionString))
                    {
                        await sqlConnection.ExecuteAsync(query, data);
                    }
                }
                catch (Exception e)
                {
                    await blob.DeleteAsync();
                    this.logger.LogCritical(e.ToString());
                }
            }


        }
    }
}