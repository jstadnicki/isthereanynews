using System.Collections.Generic;
using Itan.Core.GetNewsByChannel;

namespace Itan.Core.GetUnreadNewsByChannel
{
    public interface IGetUnreadNewsByChannelCloudRepository
    {
        List<NewsViewModel> GetNewsViewModel(string requestChannelId, List<NewsHeader> newsHeaders,
            List<NewsHeaderTagViewModel> newsHeaderTagViewModels);
    }
}