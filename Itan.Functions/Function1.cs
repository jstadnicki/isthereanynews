using Itan.Functions.Workers;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Itan.Functions
{
    public class Function1
    {
        private readonly IFunction1Worker _worker;
        private readonly ILogger<Function1> _logger;

        public Function1(IFunction1Worker worker, ILogger<Function1> logger)
        {
            _worker = worker;
            _logger = logger;
        }

        [FunctionName("Function1")]
        public async Task RunAsync(
            [TimerTrigger("0 0 */6 * * *")] TimerInfo myTimer
            //[HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest r
            )
        {
            _logger.LogInformation($"Starting nameof{typeof(Function1)}");
            await _worker.Run();
            _logger.LogInformation($"Finished nameof{typeof(Function1)}");
        }
    }
}