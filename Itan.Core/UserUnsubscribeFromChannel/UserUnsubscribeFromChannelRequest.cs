using System;
using MediatR;

namespace Itan.Core.Requests
{
    public class UserUnsubscribeFromChannelRequest : IRequest
    {
        public UserUnsubscribeFromChannelRequest(string userId, string channelId)
        {
            this.UserId = Guid.Parse(userId);
            this.ChannelId = Guid.Parse(channelId);
        }

        public Guid ChannelId { get; }

        public Guid UserId { get; }
    }
}