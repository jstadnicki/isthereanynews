using System.Threading.Tasks;
using Itan.Common;
using Itan.Wrappers;

namespace Itan.Functions.Workers
{
    public class Function1Worker : IFunction1Worker
    {
        private readonly IQueue<ChannelToDownload> _messagesCollector;
        private readonly IChannelsProvider _channelsProvider;
        private readonly ILoger<Function1Worker> _loger;

        public Function1Worker(
            ILoger<Function1Worker> loger,
            IQueue<ChannelToDownload> messagesCollector,
            IChannelsProvider channelsProvider)
        {
            Ensure.NotNull(loger, nameof(loger));
            Ensure.NotNull(messagesCollector, nameof(messagesCollector));
            Ensure.NotNull(channelsProvider, nameof(channelsProvider));

            _loger = loger;
            _messagesCollector = messagesCollector;
            _channelsProvider = channelsProvider;
        }

        public async Task Run()
        {
            var listOfChannelsToDownload = await _channelsProvider.GetAllChannelsAsync();
            await _messagesCollector.AddRangeAsync(listOfChannelsToDownload, QueuesName.ChannelToDownload);
        }
    }
}