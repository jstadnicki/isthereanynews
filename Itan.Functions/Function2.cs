 using System.Threading.Tasks;
 using Itan.Functions.Models;
 using Itan.Functions.Workers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Itan.Functions
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static async Task Run(
            [QueueTrigger(QueuesName.ChannelToDownload, Connection = "emulator")]string myQueueItem,
            ExecutionContext context,
            ILogger log)
        {
            var f2 = new Function2Worker(log, context.FunctionAppDirectory);
            await f2.Run(myQueueItem);
        }
    }
}
