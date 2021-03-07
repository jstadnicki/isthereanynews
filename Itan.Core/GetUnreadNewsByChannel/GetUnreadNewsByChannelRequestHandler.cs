using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Itan.Core.UpdateSquashNewsUpdates;
using MediatR;
using Microsoft.Graph;

namespace Itan.Core.GetUnreadNewsByChannel
{
    public class GetUnreadNewsByChannelRequestHandler : IRequestHandler<GetUnreadNewsByChannelRequest, List<NewsViewModel>>
    {
        private readonly IGetUnreadNewsByChannelRepository dataBaseRepository;
        private readonly IGetUnreadNewsByChannelCloudRepository cloudRepository;
        private readonly IReaderSettingsRepository readerSettingsRepository;

        public GetUnreadNewsByChannelRequestHandler(
            IGetUnreadNewsByChannelRepository dataBaseRepository, 
            IGetUnreadNewsByChannelCloudRepository cloudRepository, 
            IReaderSettingsRepository readerSettingsRepository)
        {
            this.dataBaseRepository = dataBaseRepository;
            this.cloudRepository = cloudRepository;
            this.readerSettingsRepository = readerSettingsRepository;
        }

        public async Task<List<NewsViewModel>> Handle(GetUnreadNewsByChannelRequest request, CancellationToken cancellationToken)
        {
            var readerSettings = await this.readerSettingsRepository.GetAsync(request.UserId);
            var newsHeaders =  await this.dataBaseRepository.GetUnreadNewsAsync(request.ChannelId, request.UserId, readerSettings.ShowUpdatedNews, readerSettings.SquashNewsUpdates);
            var newsViewModel = this.cloudRepository.GetNewsViewModel(request.ChannelId, newsHeaders);
            return newsViewModel;
        }
    }
}

