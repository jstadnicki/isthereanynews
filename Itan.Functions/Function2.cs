using Itan.Functions.Models;
using Itan.Functions.Workers;

using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;

namespace Itan.Functions
{
    public class Function2
    {
        private readonly IFunction2Worker worker;

        public Function2(IFunction2Worker worker)
        {
            this.worker = worker;
        }

        [FunctionName("Function2")]
        public async Task Run([QueueTrigger(QueuesName.ChannelToDownload, Connection = "Storage")]string myQueueItem)
        {
            await this.worker.Run(myQueueItem);
        }
    }
}
