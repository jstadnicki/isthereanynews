using System;
using MediatR;

namespace Itan.Core.MarkNewsOpen
{
    public class MarkNewsOpenCommand : IRequest
    {
        public Guid ChannelId { get; }
        public Guid NewsId { get; }
        public Guid UserId { get; }

        public MarkNewsOpenCommand(Guid channelId, Guid newsId, Guid userId)
        {
            this.ChannelId = channelId;
            this.NewsId = newsId;
            this.UserId = userId;
        }
    }
}