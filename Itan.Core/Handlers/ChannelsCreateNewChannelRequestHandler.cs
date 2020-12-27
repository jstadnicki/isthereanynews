using System;
using System.Threading;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Itan.Core.ImportSubscriptions;
using Itan.Core.Requests;
using Itan.Functions.Models;
using Itan.Wrappers;
using MediatR;

namespace Itan.Core.Handlers
{
    public class ChannelsCreateNewChannelRequestHandler : IRequestHandler<ChannelsCreateNewChannelRequest, ChannelCreateRequestResult>
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

        public async Task<ChannelCreateRequestResult> Handle(ChannelsCreateNewChannelRequest request, CancellationToken cancellationToken)
        {
            this.Validate(request);
            var uri = new Uri(request.Url.ToLowerInvariant());
            var channelToSearch = uri.Authority + uri.AbsolutePath;

            var existingChannelId = await channelFinderRepository.FindChannelIdByUrlAsync(channelToSearch, IChannelFinderRepository.Match.Like);
            if (existingChannelId == default(Guid))
            {
                var channelId = await this.repository.SaveAsync(request.Url.ToLowerInvariant(), request.PersonId);
                var mesg = new ChannelToDownload
                {
                    Id = channelId,
                    Url = request.Url
                };
                await this.messagesCollector.AddAsync(mesg, QueuesName.ChannelToDownload);
                return ChannelCreateRequestResult.Created;
            }
            return ChannelCreateRequestResult.AlreadyExists;
        }


        private void Validate(ChannelsCreateNewChannelRequest request)
        {
            try
            {
                FeedReader.Read(request.Url);
            }
            catch (Exception e)
            {
                throw new ItanValidationException(e);
            }
        }
    }

    public class ItanValidationException : Exception
    {
        public ItanValidationException(Exception exception) : base("ItanValidationException", exception)
        {
        }
    }
}