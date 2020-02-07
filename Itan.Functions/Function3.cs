using Microsoft.Azure.WebJobs;

using System;
using System.IO;
using System.Threading.Tasks;

namespace Itan.Functions
{
    public static class Function3
    {
        [FunctionName("Function3")]
        public static async Task RunAsync(
            [BlobTrigger("rss/raw/{folder}/{name}", Connection = "emulator")]
            Stream myBlob,
            Guid folder,
            string name,
            ExecutionContext context)
        {
            var worker = new Function3Worker(null, context.FunctionAppDirectory);
            await worker.RunAsync(folder, name, myBlob);
        }
    }
}
