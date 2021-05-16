using System.Collections.Generic;
using MediatR;

namespace Itan.Core.GetAllChannels
{
    public class GetAllChannelsViewModelsRequest : IRequest<List<ChannelViewModel>>
    {
    }
}