using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Core.GetReader
{
    public class GetReaderRequestHandler : IRequestHandler<GetReaderRequest, ReaderDetailsViewModel>
    {
        private readonly IReaderRepository _readerRepository;

        public GetReaderRequestHandler(IReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        public async Task<ReaderDetailsViewModel> Handle(GetReaderRequest request, CancellationToken cancellationToken)
        {
            var subscribedChannelsAsync = await _readerRepository.GetSubscribedChannelsAsync(request.Id);
            return new ReaderDetailsViewModel(subscribedChannelsAsync);
        }
    }
}