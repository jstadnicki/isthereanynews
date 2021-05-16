using System.Collections.Generic;
using MediatR;

namespace Itan.Core.GetAllSubscribedChannels
{
    public class GetAllSubscribedChannelsViewModelsRequest : IRequest<List<ChannelViewModel>>
    {
        public string PersonId { get; set; }
    }
}