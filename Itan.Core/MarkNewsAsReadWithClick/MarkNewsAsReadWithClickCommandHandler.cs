using System.Threading;
using System.Threading.Tasks;
using Itan.Core.MarkNewsRead;
using MediatR;

namespace Itan.Core.MarkNewsAsReadWithClick
{
    public class MarkNewsAsReadWithClickCommandHandler : IRequestHandler<MarkNewsAsReadWithClickCommand, Unit>
    {
        private readonly IMarkNewsReadRepository _repository;

        public MarkNewsAsReadWithClickCommandHandler(IMarkNewsReadRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(MarkNewsAsReadWithClickCommand request, CancellationToken cancellationToken)
        {
            await _repository.MarkReadAsync(request.ChannelId, request.NewsId, request.UserId, IMarkNewsReadRepository.NewsReadType.Click);
            return Unit.Value;
        }
    }
}