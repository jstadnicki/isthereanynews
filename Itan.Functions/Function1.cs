using Itan.Functions.Workers;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;

namespace Itan.Functions
{
    public class Function1
    {
        private readonly IFunction1Worker _worker;

        public Function1(IFunction1Worker worker) => _worker = worker;

        [FunctionName("Function1")]
        public async Task RunAsync([TimerTrigger("0 0 */6 * * *")] TimerInfo myTimer)
        {
            await _worker.Run();
        }
    }
}