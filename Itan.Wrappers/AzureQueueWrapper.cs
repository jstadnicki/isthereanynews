using System.Collections.Generic;
using System.Threading.Tasks;
using Itan.Common;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Itan.Wrappers
{
    public class AzureQueueWrapper<T> : IQueue<T>
    {
        private readonly ISerializer serializer;
        private readonly CloudStorageAccount storageAccount;
        private readonly CloudQueueClient queueClient;
        private CloudQueue queue;

        public AzureQueueWrapper(IOptions<ConnectionOptions> connectionOptions, ISerializer serializer)
        {
            Ensure.NotNull(connectionOptions, nameof(connectionOptions));
            Ensure.NotNull(connectionOptions.Value, nameof(connectionOptions.Value));
            Ensure.NotNull(connectionOptions.Value.SqlReader, nameof(connectionOptions.Value.SqlReader));
            Ensure.NotNull(connectionOptions.Value.SqlWriter, nameof(connectionOptions.Value.SqlWriter));
            Ensure.NotNull(connectionOptions.Value.Storage, nameof(connectionOptions.Value.Storage));

            Ensure.NotNull(serializer, nameof(serializer));

            this.serializer = serializer;
            this.storageAccount = CloudStorageAccount.Parse(connectionOptions.Value.Storage);
            this.queueClient = this.storageAccount.CreateCloudQueueClient();
            
        }

        public async Task AddRangeAsync(IEnumerable<T> elementsToAdd, string queueName)
        {
            this.queue = this.queueClient.GetQueueReference(queueName);
            await this.queue.CreateIfNotExistsAsync();

            foreach (var element in elementsToAdd)
            {
                var serializedElement = this.serializer.Serialize(element);
                await this.queue.AddMessageAsync(new CloudQueueMessage(serializedElement));
            }
        }

        public async Task AddAsync(T element, string queueName)
        {
            this.queue = this.queueClient.GetQueueReference(queueName);
            await this.queue.CreateIfNotExistsAsync();

            var serializedElement = this.serializer.Serialize(element);
            await this.queue.AddMessageAsync(new CloudQueueMessage(serializedElement));
        }
    }
}