using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Itan.Functions.Workers;

namespace Itan.Functions
{
    public class Function3
    {
        private readonly IFunction3Worker _worker;

        public Function3(IFunction3Worker worker) => _worker = worker;

        [FunctionName("Function3")]
        public async Task RunAsync(
            ILogger log,
            [BlobTrigger("rss/raw/{folder}/{name}", Connection = "Storage")]
            Stream myBlob,
            Guid folder,
            string name
        )
        {
            await _worker.RunAsync(folder, name, myBlob);
        }
    }
}