using Itan.Functions.Workers;

using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using Itan.Common;
using Microsoft.Extensions.Logging;

namespace Itan.Functions
{
    public class Function2
    {
        private readonly IFunction2Worker _worker;
        private readonly ILogger<Function2> _logger;

        public Function2(IFunction2Worker worker, ILogger<Function2> logger)
        {
            _worker = worker;
            _logger = logger;
        }

        [FunctionName("Function2")]
        [Singleton]
        public async Task Run([QueueTrigger(QueuesName.ChannelToDownload, Connection = "Storage")]string myQueueItem)
        {
            _logger.LogInformation($"Starting nameof{typeof(Function2)}");
            await _worker.Run(myQueueItem);
            _logger.LogInformation($"Finished nameof{typeof(Function2)}");
        }
        
        
    }
}
