using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Core.GetUnreadNewsByChannel
{
    public class GetUnreadNewsByChannelRequestHandler : IRequestHandler<GetUnreadNewsByChannelRequest, List<NewsViewModel>>
    {
        private readonly IGetUnreadNewsByChannelRepository dataBaseRepository;
        private readonly IGetUnreadNewsByChannelCloudRepository cloudRepository;

        public GetUnreadNewsByChannelRequestHandler(IGetUnreadNewsByChannelRepository dataBaseRepository, IGetUnreadNewsByChannelCloudRepository cloudRepository)
        {
            this.dataBaseRepository = dataBaseRepository;
            this.cloudRepository = cloudRepository;
        }

        public async Task<List<NewsViewModel>> Handle(GetUnreadNewsByChannelRequest request, CancellationToken cancellationToken)
        {
            var newsHeaders =  await this.dataBaseRepository.GetUnreadNewsAsync(request.ChannelId, request.UserId);
            var newsViewModel = this.cloudRepository.GetNewsViewModel(request.ChannelId, newsHeaders);
            return newsViewModel;
        }
    }
}