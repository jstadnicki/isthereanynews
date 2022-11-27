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
        private readonly ILogger<Function3> _logger;

        public Function3(IFunction3Worker worker, ILogger<Function3> logger)
        {
            _worker = worker;
            _logger = logger;
        }

        [FunctionName("Function3")]
        public async Task RunAsync(
            ILogger log,
            [BlobTrigger("rss/raw/{folder}/{name}", Connection = "Storage")]
            Stream myBlob,
            Guid folder,
            string name
        )
        {
            _logger.LogInformation($"Starting nameof{typeof(Function3)}");
            await _worker.RunAsync(folder, name, myBlob);
            _logger.LogInformation($"Finished nameof{typeof(Function3)}");
        }
    }
}