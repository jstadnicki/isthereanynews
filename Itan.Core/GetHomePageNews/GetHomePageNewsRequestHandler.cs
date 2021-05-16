using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Core.GetHomePageNews
{
    public class GetHomePageNewsRequestHandler : IRequestHandler<GetHomePageNewsRequest, HomePageNewsViewModel>
    {
        private IHomePageNewsRequestHandlerRepository getHomePageNewsRequestHandlerRepository;

        public GetHomePageNewsRequestHandler(IHomePageNewsRequestHandlerRepository getHomePageNewsRequestHandlerRepository)
        {
            this.getHomePageNewsRequestHandlerRepository = getHomePageNewsRequestHandlerRepository;
        }

        public async Task<HomePageNewsViewModel> Handle(GetHomePageNewsRequest request, CancellationToken cancellationToken)
        {
            var hpn = await this.getHomePageNewsRequestHandlerRepository.GetHomePageNews();
            return hpn;
        }
    }
}