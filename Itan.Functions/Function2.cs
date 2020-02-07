using Itan.Functions.Models;
using Itan.Functions.Workers;

using Microsoft.Azure.WebJobs;

using System.Threading.Tasks;

namespace Itan.Functions
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static async Task Run(
            [QueueTrigger(QueuesName.ChannelToDownload, Connection = "emulator")]string myQueueItem,
            ExecutionContext context)
        {
            var f2 = new Function2Worker(null, context.FunctionAppDirectory);
            await f2.Run(myQueueItem);
        }
    }
}
