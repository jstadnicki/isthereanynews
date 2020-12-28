using System.Threading;
using System.Threading.Tasks;
using Itan.Core.MarkNewsRead;
using MediatR;

namespace Itan.Core.MarkUnreadNewsAsRead
{
    public class MarkUnreadNewsAsReadCommandHandler:IRequestHandler<MarkUnreadNewsAsReadCommand, Unit>
    {
        private readonly IMarkNewsReadRepository repository;

        public MarkUnreadNewsAsReadCommandHandler(IMarkNewsReadRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Unit> Handle(MarkUnreadNewsAsReadCommand request, CancellationToken cancellationToken)
        {
            await this.repository.MarkReadAsync(request.ChannelId, request.NewsId, request.UserId, IMarkNewsReadRepository.NewsReadType.Skip);
            return Unit.Value;
        }
    }
}