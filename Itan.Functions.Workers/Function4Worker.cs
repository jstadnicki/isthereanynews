using System.Threading.Tasks;
using Itan.Common;
using Itan.Wrappers;

namespace Itan.Functions.Workers
{
    public class Function4Worker : IFunction4Worker
    {
        private readonly ILoger<Function4Worker> _loger;
        private readonly ISerializer _serializer;
        private readonly IChannelUpdater _channelUpdater;

        public Function4Worker(
            ILoger<Function4Worker> logger,
            ISerializer serializer,
            IChannelUpdater channelUpdater)
        {
            Ensure.NotNull(logger, nameof(logger));
            Ensure.NotNull(serializer, nameof(serializer));
            Ensure.NotNull(channelUpdater, nameof(channelUpdater));

            _loger = logger;
            _serializer = serializer;
            _channelUpdater = channelUpdater;
        }

        public async Task RunAsync(string myQueueItem)
        {
            var message = _serializer.Deserialize<ChannelUpdate>(myQueueItem);
            await _channelUpdater.Update(message);
        }
    }
}