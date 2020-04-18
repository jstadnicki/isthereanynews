namespace Itan.Functions.Workers
{
    public interface IConnectionOptions
    {
        string SqlReader { get; }
        string SqlWriter { get; }
        string Emulator { get; }
    }
}