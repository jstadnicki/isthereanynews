namespace Itan.Functions.Workers
{
    public interface ILoger<T>
    {
        void LogCritical(string toString);
    }
}