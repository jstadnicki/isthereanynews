using System;
using System.Threading;
using System.Threading.Tasks;
using Itan.Core.Handlers;
using MediatR;

namespace Itan.Core.ImportSubscriptions
{
    public class ImportSubscriptionsRequestHandler : IRequestHandler<ImportSubscriptionsRequest, Unit>
    {
        private readonly ICreateNewChannelRepository createNewChannelRepository;
        private readonly IUserToChannelSubscriptionsRepository subscriptionsRepository;
        private readonly IChannelFinderRepository channelFinderRepository;


        public ImportSubscriptionsRequestHandler(
            ICreateNewChannelRepository createNewChannelRepository,
            IUserToChannelSubscriptionsRepository subscriptionsRepository,
            IChannelFinderRepository channelFinderRepository)
        {
            this.createNewChannelRepository = createNewChannelRepository;
            this.subscriptionsRepository = subscriptionsRepository;
            this.channelFinderRepository = channelFinderRepository;
        }

        public async Task<Unit> Handle(ImportSubscriptionsRequest request, CancellationToken cancellationToken)
        {
            foreach (var outline in request.Opml.Body.Outline)
            {
                if (string.IsNullOrWhiteSpace(outline.XmlUrl))
                {
                    continue;
                }

                var channelId = await this.channelFinderRepository.FindChannelIdByUrlAsync(outline.XmlUrl.ToLowerInvariant());
                if (channelId == default(Guid))
                {
                    channelId = await this.createNewChannelRepository.SaveAsync(outline.XmlUrl.ToLowerInvariant(), request.UserId);
                }

                await this.subscriptionsRepository.CreateSubscriptionAsync(channelId, request.UserId);
            }

            return Unit.Value;
        }
    }
}