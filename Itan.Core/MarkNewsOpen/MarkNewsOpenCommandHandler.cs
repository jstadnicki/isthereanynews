using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Core.MarkNewsOpen
{
    public class MarkNewsOpenCommandHandler : AsyncRequestHandler<MarkNewsOpenCommand>
    {
        private readonly IMarkNewsOpenRepository repository;

        public MarkNewsOpenCommandHandler(IMarkNewsOpenRepository repository)
        {
            this.repository = repository;
        }

        protected override async Task Handle(MarkNewsOpenCommand request, CancellationToken cancellationToken)
        {
            await this.repository.MarkNewsOpenAsync(request.ChannelId, request.NewsId, request.UserId);
        }
    }
}