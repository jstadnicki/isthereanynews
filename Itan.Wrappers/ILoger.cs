namespace Itan.Wrappers
{
    public interface ILoger<T>
    {
        void LogCritical(string toString);
        void LogInformation(string toString);
    }
}