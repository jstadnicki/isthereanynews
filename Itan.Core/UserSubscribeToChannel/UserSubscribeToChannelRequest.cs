using System;
using MediatR;

namespace Itan.Core.UserSubscribeToChannel
{
    public class UserSubscribeToChannelRequest : IRequest
    {
        public UserSubscribeToChannelRequest(string userId, string channelId)
        {
            UserId = Guid.Parse(userId);
            ChannelId = Guid.Parse(channelId);
        }

        public Guid UserId { get; }
        public Guid ChannelId { get; }
    }
}