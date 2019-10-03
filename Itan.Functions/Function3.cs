using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Itan.Functions
{
    public static class Function3
    {
        [FunctionName("Function3")]
        public static void Run(
            [BlobTrigger("rss/raw/{folder}/{name}", Connection = "emulator")]
            Stream myBlob, 
            Guid folder,
            string name,
            ILogger log)
        {
            //log.LogInformation($"C# Blob trigger function Processed blob\n {folder.ToString()} Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
