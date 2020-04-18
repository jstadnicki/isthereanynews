namespace Itan.Functions.Workers
{
   public class JsonWrapperSerializer : ISerializer
    {
        public T Deserialize<T>(string myQueueItem)
        {
            var item = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(myQueueItem);
            return item;
        }
    }
}