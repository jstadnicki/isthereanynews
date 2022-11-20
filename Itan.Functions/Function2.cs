using Itan.Functions.Workers;

using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using Itan.Common;

namespace Itan.Functions
{
    public class Function2
    {
        private readonly IFunction2Worker _worker;

        public Function2(IFunction2Worker worker)
        {
             _worker = worker;
        }

        [FunctionName("Function2")]
        public async Task Run([QueueTrigger(QueuesName.ChannelToDownload, Connection = "Storage")]string myQueueItem)
        {
            await _worker.Run(myQueueItem);
        }
    }
}
