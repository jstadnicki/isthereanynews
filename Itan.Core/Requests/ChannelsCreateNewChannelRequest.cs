using System;
using MediatR;

namespace Itan.Core.Requests
{
    public class ChannelsCreateNewChannelRequest : IRequest<ChannelCreateRequestResult>
    {
        public string Url { get; set; }
        public Guid PersonId { get; set; }
    }
}