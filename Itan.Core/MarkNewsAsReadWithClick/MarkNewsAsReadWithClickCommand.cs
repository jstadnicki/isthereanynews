using System;
using MediatR;

namespace Itan.Core.MarkNewsAsReadWithClick
{
    public class MarkNewsAsReadWithClickCommand : IRequest
    {
        public Guid ChannelId { get; }
        public Guid NewsId { get; }
        public Guid UserId { get; }

        public MarkNewsAsReadWithClickCommand(Guid channelId, Guid newsId, Guid userId)
        {
            ChannelId = channelId;
            NewsId = newsId;
            UserId = userId;
        }
    }
}