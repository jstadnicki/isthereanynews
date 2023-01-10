using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Itan.Common;
using Itan.Core.GetNewsByChannel;

namespace Itan.Core.GetUnreadNewsByChannel
{
    public interface IGetUnreadNewsByChannelRepository
    {
        Task<List<NewsHeader>> GetUnreadNewsAsync(string channelId, string personId, UpdatedNews updatedNews, SquashUpdate squashUpdate);
        Task<List<NewsHeaderTagViewModel>> GetTagsForNewsAsync(List<Guid> newsId);
    }
}