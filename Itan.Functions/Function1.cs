using Itan.Functions.Models;
using Itan.Functions.Workers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Itan.Functions
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task RunAsync(
            ILogger log,
            [TimerTrigger("0 * */1 * * *")] TimerInfo myTimer,
            ExecutionContext context,
            [Queue(QueuesName.ChannelToDownload, Connection = "emulator")]
            IAsyncCollector<ChannelToDownload> messagesCollector)
        {
            IQueue<ChannelToDownload> queueWrapper = new AzureQueueWrapper<ChannelToDownload>(messagesCollector);

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = config.GetConnectionString("sql-itan-reader");
            
            var channelsProvider = new ChannelProvider(connectionString);
            var loger = new Loger(log);
            var f1 = new Function1Worker(loger, queueWrapper, channelsProvider);
            await f1.Run();
        }
    }
}