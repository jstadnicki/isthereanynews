using System;
using MediatR;

namespace Itan.Core.MarkNewsRead
{
    public class MarkNewsReadCommand : IRequest
    {
        public Guid ChannelId { get; }
        public Guid NewsId { get; }
        public Guid UserId { get; }

        public MarkNewsReadCommand(Guid channelId, Guid newsId, Guid userId)
        {
            this.ChannelId = channelId;
            this.NewsId = newsId;
            this.UserId = userId;
        }
    }
}