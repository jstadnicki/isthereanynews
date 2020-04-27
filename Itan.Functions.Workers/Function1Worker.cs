using System.Threading.Tasks;
using Itan.Functions.Models;
using Itan.Functions.Workers.Wrappers;

namespace Itan.Functions.Workers
{
    public class Function1Worker : IFunction1Worker
    {
        private readonly IQueue<ChannelToDownload> messagesCollector;
        private readonly IChannelsProvider channelsProvider;
        private readonly ILoger<Function1Worker> loger;

        public Function1Worker(
            ILoger<Function1Worker> loger,
            IQueue<ChannelToDownload> messagesCollector,
            IChannelsProvider channelsProvider)
        {
            Ensure.NotNull(loger, nameof(loger));
            Ensure.NotNull(messagesCollector, nameof(messagesCollector));
            Ensure.NotNull(channelsProvider, nameof(channelsProvider));

            this.loger = loger;
            this.messagesCollector = messagesCollector;
            this.channelsProvider = channelsProvider;
        }

        public async Task Run()
        {
            var listOfChannelsToDownload = await this.channelsProvider.GetAllChannelsAsync();
            await this.messagesCollector.AddRangeAsync(listOfChannelsToDownload);
        }
    }
}