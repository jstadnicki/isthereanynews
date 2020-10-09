using System.Collections.Generic;
using MediatR;

namespace Itan.Core.GetUnreadNewsByChannel
{
    public class GetUnreadNewsByChannelRequest : IRequest<List<NewsViewModel>>
    {
        public string ChannelId { get; }
        public string UserId { get; }

        public GetUnreadNewsByChannelRequest(string channelId, string userId)
        {
            this.ChannelId = channelId;
            this.UserId = userId;
        }
    }
}