namespace Itan.Functions.Workers
{
    public class ConnectionOptions : IConnectionOptions
    {
        public string SqlReader { get; set; }
        public string SqlWriter { get; set; }
        public string Emulator { get; set; }
    }
}