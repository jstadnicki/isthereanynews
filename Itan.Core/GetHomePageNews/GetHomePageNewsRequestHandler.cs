using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Core.GetHomePageNews
{
    public class GetHomePageNewsRequestHandler : IRequestHandler<GetHomePageNewsRequest, HomePageNewsViewModel>
    {
        private IHomePageNewsRequestHandlerRepository _getHomePageNewsRequestHandlerRepository;

        public GetHomePageNewsRequestHandler(IHomePageNewsRequestHandlerRepository getHomePageNewsRequestHandlerRepository)
        {
            _getHomePageNewsRequestHandlerRepository = getHomePageNewsRequestHandlerRepository;
        }

        public async Task<HomePageNewsViewModel> Handle(GetHomePageNewsRequest request, CancellationToken cancellationToken)
        {
            var hpn = await _getHomePageNewsRequestHandlerRepository.GetHomePageNews();
            return hpn;
        }
    }
}