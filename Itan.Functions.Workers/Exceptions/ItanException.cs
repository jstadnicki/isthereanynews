using System;

namespace Itan.Functions.Workers.Exceptions
{
    public class ItanException : Exception
    {
        protected ItanException(string name, Exception exception) : base(name, exception)
        {
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}