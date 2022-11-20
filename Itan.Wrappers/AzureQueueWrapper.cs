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
        private readonly ISerializer _serializer;
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudQueueClient _queueClient;
        private CloudQueue _queue;

        public AzureQueueWrapper(IOptions<ConnectionOptions> connectionOptions, ISerializer serializer)
        {
            Ensure.NotNull(connectionOptions, nameof(connectionOptions));
            Ensure.NotNull(connectionOptions.Value, nameof(connectionOptions.Value));
            Ensure.NotNull(connectionOptions.Value.SqlReader, nameof(connectionOptions.Value.SqlReader));
            Ensure.NotNull(connectionOptions.Value.SqlWriter, nameof(connectionOptions.Value.SqlWriter));
            Ensure.NotNull(connectionOptions.Value.Storage, nameof(connectionOptions.Value.Storage));

            Ensure.NotNull(serializer, nameof(serializer));

            _serializer = serializer;
            _storageAccount = CloudStorageAccount.Parse(connectionOptions.Value.Storage);
            _queueClient = _storageAccount.CreateCloudQueueClient();
            
        }

        public async Task AddRangeAsync(IEnumerable<T> elementsToAdd, string queueName)
        {
            _queue = _queueClient.GetQueueReference(queueName);
            await _queue.CreateIfNotExistsAsync();

            foreach (var element in elementsToAdd)
            {
                var serializedElement = _serializer.Serialize(element);
                await _queue.AddMessageAsync(new CloudQueueMessage(serializedElement));
            }
        }

        public async Task AddAsync(T element, string queueName)
        {
            _queue = _queueClient.GetQueueReference(queueName);
            await _queue.CreateIfNotExistsAsync();

            var serializedElement = _serializer.Serialize(element);
            await _queue.AddMessageAsync(new CloudQueueMessage(serializedElement));
        }
    }
}