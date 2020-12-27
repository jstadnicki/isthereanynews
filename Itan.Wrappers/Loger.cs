using Itan.Common;
using Microsoft.Extensions.Logging;

namespace Itan.Wrappers
{
    public class Loger<T> : ILoger<T>
    {
        private readonly ILogger<T> log;

        public Loger(ILogger<T> log)
        {
            Ensure.NotNull(log, nameof(log));
            this.log = log;
        }

        public void LogCritical(string toString) => this.log.LogCritical(toString);
    }
}