using System.Threading.Tasks;
using Itan.Functions.Models;

namespace Itan.Functions.Workers
{
    public class Function1Worker
    {
        private readonly IQueue<ChannelToDownload> messagesCollector;
        private readonly IChannelsProvider channelsProvider;
        private readonly ILoger loger;

        public Function1Worker(
            ILoger loger,
            IQueue<ChannelToDownload> messagesCollector,
            IChannelsProvider channelsProvider)
        {
            this.loger = loger;
            this.messagesCollector = messagesCollector;
            this.channelsProvider = channelsProvider;
        }

        public async Task Run()
        {
            var listOfChannelsToDownload = await this.channelsProvider.GetAllChannelsAsync();
            await messagesCollector.AddRangeAsync(listOfChannelsToDownload);
        }
    }
}