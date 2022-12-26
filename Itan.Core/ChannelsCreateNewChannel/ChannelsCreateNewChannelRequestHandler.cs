using System;
using System.Threading;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Itan.Common;
using Itan.Core.ImportSubscriptions;
using Itan.Wrappers;
using MediatR;

namespace Itan.Core.ChannelsCreateNewChannel
{
    public class
        ChannelsCreateNewChannelRequestHandler : IRequestHandler<ChannelsCreateNewChannelRequest,
            ChannelCreateRequestResult>
    {
        private readonly ICreateNewChannelRepository _repository;
        private readonly IQueue _messagesCollector;
        private readonly IChannelFinderRepository _channelFinderRepository;

        public ChannelsCreateNewChannelRequestHandler(
            ICreateNewChannelRepository repository,
            IQueue messagesCollector,
            IChannelFinderRepository channelFinderRepository)
        {
            _repository = repository;
            _messagesCollector = messagesCollector;
            _channelFinderRepository = channelFinderRepository;
        }

        public async Task<ChannelCreateRequestResult> Handle(
            ChannelsCreateNewChannelRequest request,
            CancellationToken _)
        {
            var feed = await FeedReader.ReadAsync(request.Url);
            var uri = new Uri(request.Url.ToLowerInvariant());

            var title = string.IsNullOrWhiteSpace(feed.Title) ? uri.ToString() : feed.Title;
            var channelToSearch = uri.Authority + uri.AbsolutePath;

            var existingChannelId =
                await _channelFinderRepository.FindChannelIdByUrlAsync(channelToSearch, IChannelFinderRepository.Match.Like);
            if (existingChannelId != default(Guid))
            {
                return ChannelCreateRequestResult.Exists(title);
            }
            var channelId = await _repository.SaveAsync(request.Url.ToLowerInvariant(), request.PersonId);
            var mesg = new ChannelToDownload()
            {
                Id = channelId,
                Url = request.Url
            };
            await _messagesCollector.AddAsync(mesg, QueuesName.ChannelToDownload);
            return ChannelCreateRequestResult.Created(title);
        }
    }
}