using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Core.GetReader
{
    public class GetReaderRequest : IRequest<ReaderDetailsViewModel>
    {
        public string Id { get; }

        public GetReaderRequest(string id)
        {
            Id = id;
        }
    }
    
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

    public class ReaderDetailsViewModel
    {
        public List<string> SubscribedChannels { get; }

        public ReaderDetailsViewModel(List<string> subscribedChannels)
        {
            SubscribedChannels = subscribedChannels;
        }
    }
}