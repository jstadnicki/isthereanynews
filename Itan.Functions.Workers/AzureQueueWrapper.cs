using System.Collections.Generic;
using System.Threading.Tasks;
using Itan.Functions.Models;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Itan.Functions.Workers
{
    public class AzureQueueWrapper<T> : IQueue<T>
    {
        private readonly ISerializer serializer;
        private CloudStorageAccount storageAccount;
        private CloudQueueClient queueClient;
        private CloudQueue queue;

        public AzureQueueWrapper(IOptions<ConnectionOptions> connectionOptions, ISerializer serializer)
        {
            this.serializer = serializer;
            this.storageAccount = CloudStorageAccount.Parse(connectionOptions.Value.Emulator);
            this.queueClient = storageAccount.CreateCloudQueueClient();
            this.queue = queueClient.GetQueueReference(QueuesName.ChannelToDownload);
            queue.CreateIfNotExistsAsync().GetAwaiter().GetResult();
        }

        public async Task AddRangeAsync(IEnumerable<T> elementsToAdd)
        {
            foreach (var element in elementsToAdd)
            {
                var serializedElement = this.serializer.Serialize(element);
                await this.queue.AddMessageAsync(new CloudQueueMessage(serializedElement));
            }
        }
    }
}