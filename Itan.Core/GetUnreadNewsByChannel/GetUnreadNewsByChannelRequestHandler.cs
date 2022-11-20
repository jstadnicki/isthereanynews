using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Itan.Core.GetAllSubscribedChannels;
using Itan.Core.GetNewsByChannel;
using Itan.Core.UpdateSquashNewsUpdates;
using MediatR;
using Microsoft.Graph;

namespace Itan.Core.GetUnreadNewsByChannel
{
    public class GetUnreadNewsByChannelRequestHandler : IRequestHandler<GetUnreadNewsByChannelRequest, List<NewsViewModel>>
    {
        private readonly IGetUnreadNewsByChannelRepository _dataBaseRepository;
        private readonly IGetUnreadNewsByChannelCloudRepository _cloudRepository;
        private readonly IReaderSettingsRepository _readerSettingsRepository;

        public GetUnreadNewsByChannelRequestHandler(
            IGetUnreadNewsByChannelRepository dataBaseRepository, 
            IGetUnreadNewsByChannelCloudRepository cloudRepository, 
            IReaderSettingsRepository readerSettingsRepository)
        {
            _dataBaseRepository = dataBaseRepository;
            _cloudRepository = cloudRepository;
            _readerSettingsRepository = readerSettingsRepository;
        }

        public async Task<List<NewsViewModel>> Handle(GetUnreadNewsByChannelRequest request, CancellationToken cancellationToken)
        {
            var readerSettings = await _readerSettingsRepository.GetAsync(request.UserId);
            var newsHeaders =  await _dataBaseRepository.GetUnreadNewsAsync(request.ChannelId, request.UserId, readerSettings.ShowUpdatedNews, readerSettings.SquashNewsUpdates);
            var newsViewModel = _cloudRepository.GetNewsViewModel(request.ChannelId, newsHeaders);
            return newsViewModel;
        }
    }
}

