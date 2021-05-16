using System.Collections.Generic;
using MediatR;

namespace Itan.Core.GetAllSubscribedChannels
{
    public class GetAllSubscribedChannelsViewModelsRequest : IRequest<List<SubscribedChannelViewModel>>
    {
        public string PersonId { get; set; }
    }
}