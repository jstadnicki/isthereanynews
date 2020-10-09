using System.Collections.Generic;

namespace Itan.Core.GetUnreadNewsByChannel
{
    public interface IGetUnreadNewsByChannelCloudRepository
    {
        List<NewsViewModel> GetNewsViewModel(string requestChannelId, List<NewsHeader> newsHeaders);
    }
}