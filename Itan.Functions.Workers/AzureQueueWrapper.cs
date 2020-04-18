using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

public class AzureQueueWrapper<T> : IQueue<T>
{
    private readonly IAsyncCollector<T> messagesCollector;

    public AzureQueueWrapper(IAsyncCollector<T> messagesCollector)
    {
        this.messagesCollector = messagesCollector;
    }

    public async Task AddRangeAsync(IEnumerable<T> elementsToAdd)
    {
        foreach (var element in elementsToAdd)
        {
            await messagesCollector.AddAsync(element);
        }
    }
}