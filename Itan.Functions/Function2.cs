using Itan.Functions.Models;
using Itan.Functions.Workers;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Itan.Functions
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static async Task Run(
            ILogger log,
            [QueueTrigger(QueuesName.ChannelToDownload, Connection = "emulator")]string myQueueItem,
            ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var sqlConnectionStringReader = config.GetConnectionString("sql-itan-reader");
            var sqlConnectionStringWrite = config.GetConnectionString("sql-itan-writer");

            var emulatorConnectionString = config.GetConnectionString("emulator");
            var f2 = new Function2Worker(
                new Loger(log),
                new ChannelsDownloadsReader(sqlConnectionStringReader),
                new BlobPathGenerator(),
                new HttpDownloader(new Loger(log)),
                new BlobContainer(emulatorConnectionString),
                new ChannelsDownloadsWriter(
                    sqlConnectionStringWrite,
                    new Loger(log)),
                new JsonWrapperSerializer());
            
            await f2.Run(myQueueItem);
        }
    }
}
