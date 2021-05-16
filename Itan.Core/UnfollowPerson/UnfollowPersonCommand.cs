using System;
using MediatR;

namespace Itan.Core.UnfollowPerson
{
    public class UnfollowPersonCommand : IRequest<Unit>
    {
        public string TargetPersonId { get; }
        public Guid ActualPersonId { get; }

        public UnfollowPersonCommand(string targetPersonId, Guid actualPersonId)
        {
            TargetPersonId = targetPersonId;
            ActualPersonId = actualPersonId;
        }
    }
}