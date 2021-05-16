using System;
using Itan.Core.Requests;
using MediatR;

namespace Itan.Core.ChannelsCreateNewChannel
{
    public class ChannelsCreateNewChannelRequest : IRequest<ChannelCreateRequestResult>
    {
        public string Url { get; set; }
        public Guid PersonId { get; set; }
    }
}