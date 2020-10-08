using Itan.Functions.Models;
using Itan.Functions.Workers;

using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;


namespace Itan.Functions
{
    public class Function4
    {
        private readonly IFunction4Worker worker;

        public Function4(IFunction4Worker worker)
        {
            this.worker = worker;
        }
        [FunctionName("Function4")]
        public async Task Run([QueueTrigger(QueuesName.ChannelUpdate, Connection = "Emulator")]string myQueueItem)
        {
            await this.worker.RunAsync(myQueueItem);
        }
    }
}
