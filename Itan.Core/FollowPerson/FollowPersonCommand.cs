using System;
using MediatR;

namespace Itan.Core.FollowPerson
{
    public class FollowPersonCommand : IRequest<Unit>
    {
        public string TargetPersonId { get; }
        public Guid ActualPersonId { get; }

        public FollowPersonCommand(string targetPersonId, Guid actualPersonId)
        {
            TargetPersonId = targetPersonId;
            ActualPersonId = actualPersonId;
        }
    }
}