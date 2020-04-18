using System.Collections.Generic;
using System.Threading.Tasks;
using Itan.Functions.Models;

public interface IChannelsProvider
{
    Task<List<ChannelToDownload>> GetAllChannelsAsync();
}