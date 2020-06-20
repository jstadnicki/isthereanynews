using System;
using System.Threading;
using System.Threading.Tasks;
using Itan.Core.CreateNewUser;
using Itan.Core.Handlers;
using MediatR;

namespace Itan.Core.Requests
{
    public class UserUnsubscribeFromChannelRequest : IRequest
    {
        public UserUnsubscribeFromChannelRequest(string userId, string channelId)
        {
            this.UserId = Guid.Parse(userId);
            this.ChannelId = Guid.Parse(channelId);
        }

        public Guid ChannelId { get; }

        public Guid UserId { get; }
    }

    public class UserUnsubscribeFromChannelRequestHandler : IRequestHandler<UserUnsubscribeFromChannelRequest>
    {
        private IUnsubscribeFromChannelRepository repository;

        public UserUnsubscribeFromChannelRequestHandler(IUnsubscribeFromChannelRepository repository)
        {
            this.repository = repository;
        }

        public Task<Unit> Handle(UserUnsubscribeFromChannelRequest request, CancellationToken cancellationToken)
        {
            this.Validate(request);
            this.repository.DeleteSubscriptionAsync(request.ChannelId, request.UserId);
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