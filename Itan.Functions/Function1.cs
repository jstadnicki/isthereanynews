using Itan.Functions.Models;
using Itan.Functions.Workers;

using Microsoft.Azure.WebJobs;

using System.Threading.Tasks;

namespace Itan.Functions
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task Run(
            [TimerTrigger("0 * */1 * * *")]TimerInfo myTimer,
            ExecutionContext context,
            [Queue(QueuesName.ChannelToDownload, Connection = "emulator")]IAsyncCollector<ChannelToDownload> messagesCollector
            )
        {
            var f1 = new Function1Worker(null, context.FunctionAppDirectory, messagesCollector);
            await f1.Run();
        }
    }
}