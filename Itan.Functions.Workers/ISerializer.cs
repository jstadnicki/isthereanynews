namespace Itan.Functions.Workers
{
    public interface ISerializer
    {
        T Deserialize<T>(string myQueueItem);
    }
}