using System.Collections.Generic;
using System.Threading.Tasks;
using Itan.Common;

namespace Itan.Core.GetUnreadNewsByChannel
{
    public interface IGetUnreadNewsByChannelRepository
    {
        Task<List<NewsHeader>> GetUnreadNewsAsync(string channelId, string personId, UpdatedNews updatedNews, SquashUpdate squashUpdate);
    }
}