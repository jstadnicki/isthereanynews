using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Core.GetNewsByChannel
{
    public class GetNewsByChannelRequestHandler : IRequestHandler<GetNewsByChannelRequest, List<NewsViewModel>>
    {
        private INewsByChannelRequestHandlerRepository _newsByChannelRequestHandlerRepository;

        public GetNewsByChannelRequestHandler(INewsByChannelRequestHandlerRepository newsByChannelRequestHandlerRepository)
        {
            _newsByChannelRequestHandlerRepository = newsByChannelRequestHandlerRepository;
        }

        public async Task<List<NewsViewModel>> Handle(GetNewsByChannelRequest request, CancellationToken cancellationToken)
        {
            var result = await _newsByChannelRequestHandlerRepository.GetAllByChannel(request.ChannelId);
            return result;
        }
    }
}