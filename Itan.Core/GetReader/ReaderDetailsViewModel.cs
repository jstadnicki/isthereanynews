using System.Collections.Generic;

namespace Itan.Core.GetReader
{
    public class ReaderDetailsViewModel
    {
        public List<ReaderSubscribedChannel> SubscribedChannels { get; }

        public ReaderDetailsViewModel(List<ReaderSubscribedChannel> subscribedChannels)
        {
            SubscribedChannels = subscribedChannels;
        }
    }
}