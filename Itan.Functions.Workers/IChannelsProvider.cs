using System.Collections.Generic;
using System.Threading.Tasks;
using Itan.Common;

namespace Itan.Functions.Workers
{
    public interface IChannelsProvider
    {
        Task<List<ChannelToDownload>> GetAllChannelsAsync();
    }
}