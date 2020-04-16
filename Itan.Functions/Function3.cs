using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Itan.Functions
{
    public static class Function3
    {
        [FunctionName("Function3")]
        public static async Task RunAsync(
            ILogger log,
            [BlobTrigger("rss/raw/{folder}/{name}", Connection = "emulator")]
            Stream myBlob,
            Guid folder,
            string name,
            ExecutionContext context)
        {
            var worker = new Function3Worker(log, context.FunctionAppDirectory);
            await worker.RunAsync(folder, name, myBlob);
        }
    }
}
