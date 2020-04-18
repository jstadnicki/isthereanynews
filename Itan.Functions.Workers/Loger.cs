using Microsoft.Extensions.Logging;

namespace Itan.Functions.Workers
{
    public class Loger : ILoger
    {
        private readonly ILogger log;

        public Loger(ILogger log)
        {
            this.log = log;
        }

        public void LogCritical(string toString) => this.log.LogCritical(toString);
    }
}