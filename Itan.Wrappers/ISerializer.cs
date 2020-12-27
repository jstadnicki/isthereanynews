namespace Itan.Wrappers
{
    public interface ISerializer
    {
        T Deserialize<T>(string myQueueItem);
        string Serialize<T>(T element);
    }
}