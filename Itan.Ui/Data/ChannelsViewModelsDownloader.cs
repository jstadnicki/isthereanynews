using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Itan.Application;

namespace Itan.Ui.Data
{
    public class ChannelsViewModelsDownloader
    {
        public static async Task<List<ChannelViewModel>> GetAllChannels()
        {
            HttpClient client = new HttpClient();
            var channelsJson = await client.GetStringAsync("http://localhost:51193/api/channels");
            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ChannelViewModel>>(channelsJson);
            return list;
        }
    }
}