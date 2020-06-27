using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Core.Requests
{
    public class GetNewsByChannelRequestHandler : IRequestHandler<GetNewsByChannelRequest, List<NewsViewModel>>
    {
        private INewsByChannelRequestHandlerRepository newsByChannelRequestHandlerRepository;

        public GetNewsByChannelRequestHandler(INewsByChannelRequestHandlerRepository newsByChannelRequestHandlerRepository)
        {
            this.newsByChannelRequestHandlerRepository = newsByChannelRequestHandlerRepository;
        }

        public async Task<List<NewsViewModel>> Handle(GetNewsByChannelRequest request, CancellationToken cancellationToken)
        {
            var result = await this.newsByChannelRequestHandlerRepository.GetAllByChannel(request.ChannelId);
            return result;
        }
    }
}