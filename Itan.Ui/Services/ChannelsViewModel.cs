using Itan.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Itan.Ui.Services
{
    public class ChannelsViewModel
    {
        public ChannelsViewModel()
        {
            this.news = new List<NewsContentViewModel>();
            this.channels = new List<ChannelViewModel>();
        }

        private HttpClient client;
        private List<NewsContentViewModel> news;
        private List<ChannelViewModel> channels;

        public List<ChannelViewModel> Channels => channels.ToList();
        public List<NewsContentViewModel> News => news.ToList();

        public async Task OnInitializedAsync()
        {
            this.client = new HttpClient();
            var stringChannels = await client.GetStringAsync("http://localhost:51193/api/channels");
            this.channels = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ChannelViewModel>>(stringChannels);
        }

        public async Task OnChannelClickedAsync(Guid channelId)
        {
            var stringChannelNews = await client.GetStringAsync($"http://localhost:51193/api/news/{channelId}");
            var newsViewModels = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NewsViewModel>>(stringChannelNews);
            this.news = newsViewModels.Select(x => new NewsContentViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Url = x.ContentUrl,
                Downloaded = false,
                Content = string.Empty
            }).ToList();
        }
    }
}
