using System;
using MediatR;

namespace Itan.Api.Controllers
{
    public class UserSubscribeToChannelRequest : IRequest
    {
        public UserSubscribeToChannelRequest(string userId, string channelId)
        {
            this.UserId = Guid.Parse(userId);
            this.ChannelId = Guid.Parse(channelId);
        }

        public Guid UserId { get; }
        public Guid ChannelId { get; }
    }
}