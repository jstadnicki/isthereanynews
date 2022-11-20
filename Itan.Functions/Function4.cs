using Itan.Functions.Workers;

using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using Itan.Common;


namespace Itan.Functions
{
    public class Function4
    {
        private readonly IFunction4Worker _worker;

        public Function4(IFunction4Worker worker)
        {
            _worker = worker;
        }
        [FunctionName("Function4")]
        public async Task Run([QueueTrigger(QueuesName.ChannelUpdate, Connection = "Storage")]string myQueueItem)
        {
            await _worker.RunAsync(myQueueItem);
        }
    }
}
