using System;
using System.Threading;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Itan.Core.Requests;
using MediatR;

namespace Itan.Core.Handlers
{
    public class ChannelsCreateNewChannelRequestHandler : IRequestHandler<ChannelsCreateNewChannelRequest, ChannelCreateRequestResult>
    {
        private readonly ICreateNewChannelRepository repository;

        public ChannelsCreateNewChannelRequestHandler(ICreateNewChannelRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ChannelCreateRequestResult> Handle(ChannelsCreateNewChannelRequest request, CancellationToken cancellationToken)
        {
            this.Validate(request);
            await this.repository.SaveAsync(request.Url, request.PersonId);
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