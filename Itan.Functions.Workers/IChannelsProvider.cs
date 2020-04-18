using System.Collections.Generic;
using System.Threading.Tasks;
using Itan.Functions.Models;

namespace Itan.Functions.Workers
{
    public interface IChannelsProvider
    {
        Task<List<ChannelToDownload>> GetAllChannelsAsync();
    }
}