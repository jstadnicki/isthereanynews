using System;
using System.Threading;
using System.Threading.Tasks;
using Itan.Core.Handlers;
using Itan.Functions.Models;
using Itan.Wrappers;
using MediatR;

namespace Itan.Core.ImportSubscriptions
{
    public class ImportSubscriptionsRequestHandler : IRequestHandler<ImportSubscriptionsRequest, Unit>
    {
        private readonly ICreateNewChannelRepository createNewChannelRepository;
        private readonly IUserToChannelSubscriptionsRepository subscriptionsRepository;
        private readonly IChannelFinderRepository channelFinderRepository;
        private readonly IQueue<ChannelToDownload> messagesCollector;


        public ImportSubscriptionsRequestHandler(
            ICreateNewChannelRepository createNewChannelRepository,
            IUserToChannelSubscriptionsRepository subscriptionsRepository,
            IChannelFinderRepository channelFinderRepository,
            IQueue<ChannelToDownload> messagesCollector)
        {
            this.createNewChannelRepository = createNewChannelRepository;
            this.subscriptionsRepository = subscriptionsRepository;
            this.channelFinderRepository = channelFinderRepository;
            this.messagesCollector = messagesCollector;
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

                var channelId = await this.channelFinderRepository.FindChannelIdByUrlAsync(channelToSearch, IChannelFinderRepository.Match.Like);
                if (channelId == default(Guid))
                {
                    channelId = await this.createNewChannelRepository.SaveAsync(outline.XmlUrl.ToLowerInvariant(), request.UserId);
                }
                var message = new ChannelToDownload
                {
                    Id = channelId,
                    Url = outline.XmlUrl.ToLowerInvariant()
                };
                await this.messagesCollector.AddAsync(message,QueuesName.ChannelToDownload);
                await this.subscriptionsRepository.CreateSubscriptionAsync(channelId, request.UserId);
            }

            return Unit.Value;
        }
    }
}