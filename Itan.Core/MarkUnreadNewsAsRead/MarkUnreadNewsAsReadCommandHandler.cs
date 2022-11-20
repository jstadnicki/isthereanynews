using System.Threading;
using System.Threading.Tasks;
using Itan.Core.MarkNewsRead;
using MediatR;

namespace Itan.Core.MarkUnreadNewsAsRead
{
    public class MarkUnreadNewsAsReadCommandHandler:IRequestHandler<MarkUnreadNewsAsReadCommand, Unit>
    {
        private readonly IMarkNewsReadRepository _repository;

        public MarkUnreadNewsAsReadCommandHandler(IMarkNewsReadRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(MarkUnreadNewsAsReadCommand request, CancellationToken cancellationToken)
        {
            await _repository.MarkReadAsync(request.ChannelId, request.NewsId, request.UserId, IMarkNewsReadRepository.NewsReadType.Skip);
            return Unit.Value;
        }
    }
}