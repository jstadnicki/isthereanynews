using System.Threading.Tasks;
using Itan.Functions.Models;
using Itan.Functions.Workers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Itan.Functions
{
    public static class Function4
    {
        [FunctionName("Function4")]
        public static async Task Run(
            [QueueTrigger(QueuesName.ChannelUpdate, Connection = "emulator")]string myQueueItem,
            ExecutionContext context,
            ILogger log)
        {
            var message = JsonConvert.DeserializeObject<ChannelUpdate>(myQueueItem);
            var worker = new Function4Worker(log, context.FunctionAppDirectory);
            await worker.RunAsync(message);
        }
    }
}
