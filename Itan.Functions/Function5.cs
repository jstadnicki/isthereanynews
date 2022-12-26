using System.Threading.Tasks;
using Itan.Common;
using Itan.Functions.Workers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Itan.Functions;

public class Function5
{
    private readonly ILogger<Function5> _logger;
    private readonly IFunction5Worker _worker;

    public Function5(ILogger<Function5> logger, IFunction5Worker worker)
    {
        _logger = logger;
        _worker = worker;
    }

    [FunctionName("Function5")]
    public async Task Run([QueueTrigger(QueuesName.NewsCategories, Connection = "Storage")]string myQueueItem)
    {
        _logger.LogInformation($"Starting nameof{typeof(Function5)}");
        await _worker.Run(myQueueItem);
        _logger.LogInformation($"Finished nameof{typeof(Function5)}");
    }
}