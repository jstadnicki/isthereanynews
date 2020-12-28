using System;
using System.Collections.Generic;
using MediatR;

namespace Itan.Core.MarkUnreadNewsAsRead
{
    public class MarkUnreadNewsAsReadCommand : IRequest
    {
        public Guid ChannelId { get; }
        public ICollection<Guid> NewsId { get; }
        public Guid UserId { get; }

        public MarkUnreadNewsAsReadCommand(Guid requestChannelId, ICollection<Guid> requestNewsId, Guid userId)
        {
            ChannelId = requestChannelId;
            NewsId = requestNewsId;
            UserId = userId;
        }
    }
}