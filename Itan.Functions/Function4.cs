//using Itan.Functions.Models;
//using Itan.Functions.Workers;

//using Microsoft.Azure.WebJobs;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;

//using System.Threading.Tasks;


//namespace Itan.Functions
//{
//    public static class Function4
//    {
//        [FunctionName("Function4")]
//        public static async Task Run(
//            ILogger log,
//            [QueueTrigger(QueuesName.ChannelUpdate, Connection = "emulator")]string myQueueItem,
//            ExecutionContext context)
//        {
//            var message = JsonConvert.DeserializeObject<ChannelUpdate>(myQueueItem);
//            var worker = new Function4Worker(log, context.FunctionAppDirectory);
//            await worker.RunAsync(message);
//        }
//    }
//}
