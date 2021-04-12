using System.Collections.Generic;
using MediatR;

namespace Itan.Core
{
    public class GetAllChannelsViewModelsRequest : IRequest<List<ChannelViewModel>>
    {
    }
}