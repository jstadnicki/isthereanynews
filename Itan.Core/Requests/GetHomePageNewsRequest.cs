using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Core.Requests
{
    public class GetHomePageNewsRequest : IRequest<HomePageNews>
    {
    }

    public class GetHomePageNewsRequestHandler : IRequestHandler<GetHomePageNewsRequest, HomePageNews>
    {
        private IHomePageNewsRequestHandlerRepository getHomePageNewsRequestHandlerRepository;

        public GetHomePageNewsRequestHandler(IHomePageNewsRequestHandlerRepository getHomePageNewsRequestHandlerRepository)
        {
            this.getHomePageNewsRequestHandlerRepository = getHomePageNewsRequestHandlerRepository;
        }

        public async Task<HomePageNews> Handle(GetHomePageNewsRequest request, CancellationToken cancellationToken)
        {
            var hpn = await this.getHomePageNewsRequestHandlerRepository.GetHomePageNews();
            return hpn;
        }
    }
}