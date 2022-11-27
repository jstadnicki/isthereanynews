using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Queues;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Wrappers
{
    public class AzureQueueWrapper<T> : IQueue<T>
    {
        private readonly ISerializer _serializer;
        private readonly QueueServiceClient _queueClient;

        public AzureQueueWrapper(
            IOptions<ConnectionOptions> connectionOptions, 
            ISerializer serializer)
        {
            Ensure.NotNull(connectionOptions, nameof(connectionOptions));
            Ensure.NotNull(connectionOptions.Value, nameof(connectionOptions.Value));
            Ensure.NotNull(connectionOptions.Value.SqlReader, nameof(connectionOptions.Value.SqlReader));
            Ensure.NotNull(connectionOptions.Value.SqlWriter, nameof(connectionOptions.Value.SqlWriter));
            Ensure.NotNull(connectionOptions.Value.Storage, nameof(connectionOptions.Value.Storage));

            Ensure.NotNull(serializer, nameof(serializer));

            _serializer = serializer;
            _queueClient = new QueueServiceClient(connectionOptions.Value.Storage);
        }

        public async Task AddRangeAsync(IEnumerable<T> elementsToAdd, string queueName)
        {
            var queue = _queueClient.GetQueueClient(queueName);
            await queue.CreateIfNotExistsAsync();

            foreach (var element in elementsToAdd)
            {
                var serializedElement = _serializer.Serialize(element);
                await queue.SendMessageAsync(serializedElement);
            }
        }

        public async Task AddAsync(T element, string queueName)
        {
            var queue = _queueClient.GetQueueClient(queueName);
            await queue.CreateIfNotExistsAsync();

            var serializedElement = _serializer.Serialize(element);
            await queue.SendMessageAsync(serializedElement);
        }
    }
}