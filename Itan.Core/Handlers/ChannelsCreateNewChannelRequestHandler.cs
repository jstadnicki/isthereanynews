using System;
using System.Threading.Tasks;
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
                Console.WriteLine(e);
                throw;
            }
        }
    }
}