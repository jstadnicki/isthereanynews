using System;
using System.Threading;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
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

        public ChannelsCreateNewChannelRequestHandler(
            ICreateNewChannelRepository repository,
            IQueue<ChannelToDownload> messagesCollector)
        {
            this.repository = repository;
            this.messagesCollector = messagesCollector;
        }

        public async Task<ChannelCreateRequestResult> Handle(ChannelsCreateNewChannelRequest request, CancellationToken cancellationToken)
        {
            this.Validate(request);
            var channelId = await this.repository.SaveAsync(request.Url, request.PersonId);
            var mesg = new ChannelToDownload
            {
                Id = channelId,
                Url = request.Url
            };
            await this.messagesCollector.AddAsync(mesg,QueuesName.ChannelToDownload);
            return ChannelCreateRequestResult.Created;
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