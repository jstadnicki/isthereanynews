using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Api.Controllers
{
    public class UserSubscribeToChannelHandler : AsyncRequestHandler<UserSubscribeToChannelRequest>
    {
        protected override Task Handle(UserSubscribeToChannelRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}