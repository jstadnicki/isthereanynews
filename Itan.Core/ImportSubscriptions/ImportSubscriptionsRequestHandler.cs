using System;
using System.Threading;
using System.Threading.Tasks;
using Itan.Common;
using Itan.Core.ChannelsCreateNewChannel;
using Itan.Core.UserSubscribeToChannel;
using Itan.Wrappers;
using MediatR;

namespace Itan.Core.ImportSubscriptions
{
    public class ImportSubscriptionsRequestHandler : IRequestHandler<ImportSubscriptionsRequest, Unit>
    {
        private readonly ICreateNewChannelRepository _createNewChannelRepository;
        private readonly IUserToChannelSubscriptionsRepository _subscriptionsRepository;
        private readonly IChannelFinderRepository _channelFinderRepository;
        private readonly IQueue<ChannelToDownload> _messagesCollector;


        public ImportSubscriptionsRequestHandler(
            ICreateNewChannelRepository createNewChannelRepository,
            IUserToChannelSubscriptionsRepository subscriptionsRepository,
            IChannelFinderRepository channelFinderRepository,
            IQueue<ChannelToDownload> messagesCollector)
        {
            _createNewChannelRepository = createNewChannelRepository;
            _subscriptionsRepository = subscriptionsRepository;
            _channelFinderRepository = channelFinderRepository;
            _messagesCollector = messagesCollector;
        }

        public async Task<Unit> Handle(ImportSubscriptionsRequest request, CancellationToken cancellationToken)
        {
            foreach (var outline in request.Opml.Body.Outline)
            {
                if (string.IsNullOrWhiteSpace(outline.XmlUrl))
                {
                    continue;
                }
                var uri = new Uri(outline.XmlUrl.ToLowerInvariant());
                var channelToSearch = uri.Authority + uri.AbsolutePath;

                var channelId = await _channelFinderRepository.FindChannelIdByUrlAsync(channelToSearch, IChannelFinderRepository.Match.Like);
                if (channelId == default(Guid))
                {
                    channelId = await _createNewChannelRepository.SaveAsync(outline.XmlUrl.ToLowerInvariant(), request.UserId);
                }
                var message = new ChannelToDownload
                {
                    Id = channelId,
                    Url = outline.XmlUrl.ToLowerInvariant()
                };
                await _messagesCollector.AddAsync(message,QueuesName.ChannelToDownload);
                await _subscriptionsRepository.CreateSubscriptionAsync(channelId, request.UserId);
            }

            return Unit.Value;
        }
    }
}