using System;
using MediatR;

namespace Itan.Core.ImportSubscriptions
{
    public class ImportSubscriptionsRequest : IRequest
    {
        public Guid UserId { get; }
        public Opml Opml { get; }

        public ImportSubscriptionsRequest(Guid userId, Opml opml)
        {
            UserId = userId;
            Opml = opml;
        }
    }
}