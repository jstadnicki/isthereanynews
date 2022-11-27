using Itan.Functions.Workers;

using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using Itan.Common;
using Microsoft.Extensions.Logging;


namespace Itan.Functions
{
    public class Function4
    {
        private readonly IFunction4Worker _worker;
        private readonly ILogger<Function4> _logger;

        public Function4(IFunction4Worker worker, ILogger<Function4> logger)
        {
            _worker = worker;
            _logger = logger;
        }
        
        [FunctionName("Function4")]
        public async Task Run([QueueTrigger(QueuesName.ChannelUpdate, Connection = "Storage")]string myQueueItem)
        {
            _logger.LogInformation($"Starting nameof{typeof(Function3)}");
            await _worker.RunAsync(myQueueItem);
            _logger.LogInformation($"Finished nameof{typeof(Function3)}");
        }
    }
}
