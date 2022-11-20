using System.Collections.Generic;
using Itan.Core.GetNewsByChannel;
using MediatR;

namespace Itan.Core.GetUnreadNewsByChannel
{
    public class GetUnreadNewsByChannelRequest : IRequest<List<NewsViewModel>>
    {
        public string ChannelId { get; }
        public string UserId { get; }

        public GetUnreadNewsByChannelRequest(string channelId, string userId)
        {
            ChannelId = channelId;
            UserId = userId;
        }
    }
}