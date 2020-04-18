using Itan.Functions.Workers;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;

namespace Itan.Functions
{
    public class Function1
    {
        private readonly IFunction1Worker worker;

        public Function1(IFunction1Worker worker) => this.worker = worker;

        [FunctionName("Function1")]
        public async Task RunAsync([TimerTrigger("0 * * * */1 *")] TimerInfo myTimer)
        {
            await worker.Run();
        }
    }


}