using System;
using System.Threading;
using System.Threading.Tasks;
using Itan.Api.Controllers;
using MediatR;

namespace Itan.Core
{
    public class UserUnsubscribeFromChannelHandler : AsyncRequestHandler<UserUnsubscribeFromChannelRequest>
    {
        protected override Task Handle(UserUnsubscribeFromChannelRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}