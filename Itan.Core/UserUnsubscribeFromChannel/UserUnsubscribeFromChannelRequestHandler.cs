using System;
using System.Threading;
using System.Threading.Tasks;
using Itan.Core.CreateNewUser;
using Itan.Core.UserSubscribeToChannel;
using MediatR;

namespace Itan.Core.UserUnsubscribeFromChannel
{
    public class UserUnsubscribeFromChannelRequestHandler : IRequestHandler<UserUnsubscribeFromChannelRequest>
    {
        private IUnsubscribeFromChannelRepository _repository;

        public UserUnsubscribeFromChannelRequestHandler(IUnsubscribeFromChannelRepository repository)
        {
            _repository = repository;
        }

        public Task<Unit> Handle(UserUnsubscribeFromChannelRequest request, CancellationToken cancellationToken)
        {
            Validate(request);
            _repository.DeleteSubscriptionAsync(request.ChannelId, request.UserId);
            return Unit.Task;
        }

        private void Validate(UserUnsubscribeFromChannelRequest request)
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