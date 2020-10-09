using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itan.Core.GetUnreadNewsByChannel
{
    public interface IGetUnreadNewsByChannelRepository
    {
        Task<List<NewsHeader>> GetUnreadNewsAsync(string channelId, string personId);
    }
}