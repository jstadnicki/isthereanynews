using System;
using System.Collections.Generic;
using MediatR;

namespace Itan.Core.Requests
{
    public class GetNewsByChannelRequest : IRequest<List<NewsViewModel>>
    {
        public Guid ChannelId { get; }

        public GetNewsByChannelRequest(Guid channelId)
        {
            this.ChannelId = channelId;
        }
    }
}