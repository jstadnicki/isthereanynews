using System.Threading.Tasks;
using Itan.Functions.Models;
using Itan.Functions.Workers.Wrappers;

namespace Itan.Functions.Workers
{
    public class Function4Worker : IFunction4Worker
    {
        private ILoger<Function4Worker> loger;
        private ISerializer serializer;
        private IChannelUpdater channelUpdater;

        public Function4Worker(
            ILoger<Function4Worker> logger, 
            ISerializer serializer, 
            IChannelUpdater channelUpdater)
        {
            this.loger = logger;
            this.serializer = serializer;
            this.channelUpdater = channelUpdater;
        }

         public async Task RunAsync(string myQueueItem)
         {
            var message = this.serializer.Deserialize<ChannelUpdate>(myQueueItem);
            await this.channelUpdater.Update(message);
        }
    }
}