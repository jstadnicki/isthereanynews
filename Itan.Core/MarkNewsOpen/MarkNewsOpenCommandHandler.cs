using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Core.MarkNewsOpen
{
    public class MarkNewsOpenCommandHandler : AsyncRequestHandler<MarkNewsOpenCommand>
    {
        private readonly IMarkNewsOpenRepository _repository;

        public MarkNewsOpenCommandHandler(IMarkNewsOpenRepository repository)
        {
            _repository = repository;
        }

        protected override async Task Handle(MarkNewsOpenCommand request, CancellationToken cancellationToken)
        {
            await _repository.MarkNewsOpenAsync(request.ChannelId, request.NewsId, request.UserId);
        }
    }
}