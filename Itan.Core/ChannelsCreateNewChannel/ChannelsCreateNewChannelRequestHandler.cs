using System;
using System.Threading;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Itan.Core.ImportSubscriptions;
using Itan.Functions.Models;
using Itan.Wrappers;
using MediatR;

namespace Itan.Core.ChannelsCreateNewChannel
{
    public class
        ChannelsCreateNewChannelRequestHandler : IRequestHandler<ChannelsCreateNewChannelRequest,
            ChannelCreateRequestResult>
    {
        private readonly ICreateNewChannelRepository repository;
        private readonly IQueue<ChannelToDownload> messagesCollector;
        private readonly IChannelFinderRepository channelFinderRepository;

        public ChannelsCreateNewChannelRequestHandler(
            ICreateNewChannelRepository repository,
            IQueue<ChannelToDownload> messagesCollector,
            IChannelFinderRepository channelFinderRepository)
        {
            this.repository = repository;
            this.messagesCollector = messagesCollector;
            this.channelFinderRepository = channelFinderRepository;
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
                await channelFinderRepository.FindChannelIdByUrlAsync(channelToSearch, IChannelFinderRepository.Match.Like);
            if (existingChannelId != default(Guid))
            {
                return ChannelCreateRequestResult.Exists(title);
            }
            var channelId = await this.repository.SaveAsync(request.Url.ToLowerInvariant(), request.PersonId);
            var mesg = new ChannelToDownload
            {
                Id = channelId,
                Url = request.Url
            };
            await this.messagesCollector.AddAsync(mesg, QueuesName.ChannelToDownload);
            return ChannelCreateRequestResult.Created(title);
        }
    }
}