using System;
using System.Threading;
using System.Threading.Tasks;
using Itan.Core.CreateNewUser;
using MediatR;

namespace Itan.Core.UserSubscribeToChannel
{
    public class UserSubscribeToChannelHandler : AsyncRequestHandler<UserSubscribeToChannelRequest>
    {
        private IUserToChannelSubscriptionsRepository _repository;

        public UserSubscribeToChannelHandler(IUserToChannelSubscriptionsRepository repository)
        {
            _repository = repository;
        }

        protected override Task Handle(UserSubscribeToChannelRequest request, CancellationToken cancellationToken)
        {
            Validate(request);
            return _repository.CreateSubscriptionAsync(request.ChannelId, request.UserId);
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