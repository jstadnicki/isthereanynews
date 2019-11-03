using System.Threading.Tasks;
using Itan.Functions.Models;
using Itan.Functions.Workers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Itan.Functions
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task Run(
            [TimerTrigger("0 * */1 * * *")]TimerInfo myTimer,
            ExecutionContext context,
            [Queue(QueuesName.ChannelToDownload, Connection = "emulator")]IAsyncCollector<ChannelToDownload> messagesCollector,
            ILogger log
            )
        {
            var f1 = new Function1Worker(log, context.FunctionAppDirectory, messagesCollector);
            await f1.Run();
        }
    }
}