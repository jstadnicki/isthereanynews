using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Core.MarkNewsRead
{
    public class MarkNewsReadCommandHandler : AsyncRequestHandler<MarkNewsReadCommand>
    {
        private readonly IMarkNewsReadRepository repository;

        public MarkNewsReadCommandHandler(IMarkNewsReadRepository repository)
        {
            this.repository = repository;
        }

        protected override async Task Handle(MarkNewsReadCommand request, CancellationToken cancellationToken)
        {
            await this.repository.MarkReadAsync(request.ChannelId, request.NewsId, request.UserId, IMarkNewsReadRepository.NewsReadType.Read);
        }
    }
}