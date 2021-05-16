using System;
using System.Collections.Generic;
using MediatR;

namespace Itan.Core.GetFollowers
{
    public class GetFollowersQuery : IRequest<List<SubscribedReaderViewModel>>
    {
        public Guid FollowerPersonId { get; }

        public GetFollowersQuery(Guid followerPersonId)
        {
            FollowerPersonId = followerPersonId;
        }
    }
}