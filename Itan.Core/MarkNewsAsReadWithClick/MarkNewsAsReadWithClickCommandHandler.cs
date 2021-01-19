using System.Threading;
using System.Threading.Tasks;
using Itan.Core.MarkNewsRead;
using MediatR;

namespace Itan.Core.MarkNewsAsReadWithClick
{
    public class MarkNewsAsReadWithClickCommandHandler : IRequestHandler<MarkNewsAsReadWithClickCommand, Unit>
    {
        private readonly IMarkNewsReadRepository repository;

        public MarkNewsAsReadWithClickCommandHandler(IMarkNewsReadRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Unit> Handle(MarkNewsAsReadWithClickCommand request, CancellationToken cancellationToken)
        {
            await this.repository.MarkReadAsync(request.ChannelId, request.NewsId, request.UserId, IMarkNewsReadRepository.NewsReadType.Click);
            return Unit.Value;
        }
    }
}