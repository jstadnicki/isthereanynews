using System;
using System.Threading;
using System.Threading.Tasks;
using Itan.Api.Controllers;
using MediatR;

namespace Itan.Core.Handlers
{
    public class UserSubscribeToChannelHandler : AsyncRequestHandler<UserSubscribeToChannelRequest>
    {
        private IUserToChannelSubscriptionsRepository repository;

        public UserSubscribeToChannelHandler(IUserToChannelSubscriptionsRepository repository)
        {
            this.repository = repository;
        }

        protected override Task Handle(UserSubscribeToChannelRequest request, CancellationToken cancellationToken)
        {
            this.Validate(request);
            return this.repository.CreateSubscriptionAsync(request.ChannelId, request.UserId);
        }

        private void Validate(UserSubscribeToChannelRequest request)
        {
            if (Guid.Empty == request.UserId)
            {
                throw new BadArgumentInRequestException(nameof(UserSubscribeToChannelHandler), nameof(request.UserId));
            }
            
            if (Guid.Empty == request.ChannelId)
            {
                throw new BadArgumentInRequestException(nameof(UserSubscribeToChannelHandler), nameof(request.ChannelId));
            }
        }
    }
}