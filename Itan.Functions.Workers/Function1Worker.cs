using System.Linq;
using System.Threading.Tasks;
using Itan.Common;
using Itan.Wrappers;

namespace Itan.Functions.Workers
{
    public class Function1Worker : IFunction1Worker
    {
        private readonly IQueue _queue;
        private readonly IChannelsProvider _channelsProvider;
        private readonly ILoger<Function1Worker> _loger;

        public Function1Worker(
            ILoger<Function1Worker> loger,
            IQueue queue,
            IChannelsProvider channelsProvider
            )
        {
            Ensure.NotNull(loger, nameof(loger));
            Ensure.NotNull(queue, nameof(queue));
            Ensure.NotNull(channelsProvider, nameof(channelsProvider));

            _loger = loger;
            _queue = queue;
            _channelsProvider = channelsProvider;
        }

        public async Task Run()
        {
            _loger.LogInformation($"{nameof(Function1Worker)}-{nameof(Run)}");
            var listOfChannelsToDownload = await _channelsProvider.GetAllChannelsAsync();
            await _queue.AddRangeAsync(listOfChannelsToDownload, QueuesName.ChannelToDownload);
        }
    }
}