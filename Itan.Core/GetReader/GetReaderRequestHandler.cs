using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Core.GetReader
{
    public class GetReaderRequestHandler : IRequestHandler<GetReaderRequest, ReaderDetailsViewModel>
    {
        private readonly IReaderRepository readerRepository;

        public GetReaderRequestHandler(IReaderRepository readerRepository)
        {
            this.readerRepository = readerRepository;
        }

        public async Task<ReaderDetailsViewModel> Handle(GetReaderRequest request, CancellationToken cancellationToken)
        {
            var subscribedChannelsAsync = await this.readerRepository.GetSubscribedChannelsAsync(request.Id);
            return new ReaderDetailsViewModel(subscribedChannelsAsync);
        }
    }
}