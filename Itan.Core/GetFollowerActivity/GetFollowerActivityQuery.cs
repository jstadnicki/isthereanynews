using System.Collections.Generic;
using Itan.Core.MarkNewsRead;
using MediatR;

namespace Itan.Core.GetFollowerActivity
{
    public class GetFollowerActivityQuery : IRequest<List<FollowerActivityViewModel>>
    {
        public string PersonId { get; }

        public GetFollowerActivityQuery(string personId)
        {
            PersonId = personId;
        }
    }
}