using System;
using Itan.Common;
using MediatR;

namespace Itan.Core.UpdateSquashNewsUpdates
{
    public class UpdateSquashNewsUpdatesRequest : IRequest
    {
        public Guid UserId { get; }
        public SquashUpdate SquashNewsUpdates { get; }

        public UpdateSquashNewsUpdatesRequest(Guid userId, SquashUpdate squashNewsUpdates)
        {
            UserId = userId;
            SquashNewsUpdates = squashNewsUpdates;
        }
    }
}

