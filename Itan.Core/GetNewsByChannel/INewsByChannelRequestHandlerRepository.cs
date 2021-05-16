using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itan.Core.GetNewsByChannel
{
    public interface INewsByChannelRequestHandlerRepository
    {
        Task<List<NewsViewModel>> GetAllByChannel(Guid channelId);
    }
}