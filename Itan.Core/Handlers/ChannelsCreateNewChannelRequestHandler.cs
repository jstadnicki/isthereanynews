using System;
using CodeHollow.FeedReader;
using Itan.Core.Requests;
using MediatR;

namespace Itan.Core.Handlers
{
    public class ChannelsCreateNewChannelRequestHandler : RequestHandler<ChannelsCreateNewChannelRequest, ChannelCreateRequestResult>
    {
        private readonly ICreateNewChannelRepository repository;

        public ChannelsCreateNewChannelRequestHandler(ICreateNewChannelRepository repository)
        {
            this.repository = repository;
        }

        protected override ChannelCreateRequestResult Handle(ChannelsCreateNewChannelRequest request)
        {
            this.Validate(request);
            this.repository.Save(request.Url, request.PersonId);
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