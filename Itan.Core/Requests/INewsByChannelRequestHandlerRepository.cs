using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itan.Core.Requests
{
    public interface INewsByChannelRequestHandlerRepository
    {
        Task<List<NewsViewModel>> GetAllByChannel(Guid channelId);
    }
}