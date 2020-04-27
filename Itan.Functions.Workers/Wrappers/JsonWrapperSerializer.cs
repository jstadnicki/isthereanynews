namespace Itan.Functions.Workers.Wrappers
{
   public class JsonWrapperSerializer : ISerializer
    {
        public T Deserialize<T>(string myQueueItem) => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(myQueueItem);

        public string Serialize<T>(T element) => Newtonsoft.Json.JsonConvert.SerializeObject(element);
    }
}