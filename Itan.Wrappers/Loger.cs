using Itan.Common;
using Microsoft.Extensions.Logging;

namespace Itan.Wrappers
{
    public class Loger<T> : ILoger<T>
    {
        private readonly ILogger<T> _log;

        public Loger(ILogger<T> log)
        {
            Ensure.NotNull(log, nameof(log));
            _log = log;
        }

        public void LogCritical(string toString) => _log.LogCritical(toString);
        public void LogInformation(string toString) => _log.LogInformation(toString);
    }
}