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


        public ImportSubscriptionsRequestHandler(
            ICreateNewChannelRepository createNewChannelRepository,
            IUserToChannelSubscriptionsRepository subscriptionsRepository)
        {
            this.createNewChannelRepository = createNewChannelRepository;
            this.subscriptionsRepository = subscriptionsRepository;
        }

        public async Task<Unit> Handle(ImportSubscriptionsRequest request, CancellationToken cancellationToken)
        {
            foreach (var outline in request.Opml.Body.Outline)
            {
                var channelId =  await this.createNewChannelRepository.SaveAsync(outline.XmlUrl, request.UserId);
                await this.subscriptionsRepository.CreateSubscriptionAsync(channelId, request.UserId);
            }
            return Unit.Value;
        }
    }
}