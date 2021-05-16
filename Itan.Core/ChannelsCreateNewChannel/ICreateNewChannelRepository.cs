using System;
using System.Threading.Tasks;

namespace Itan.Core.ChannelsCreateNewChannel
{
    public interface ICreateNewChannelRepository
    {
        Task<Guid> SaveAsync(string url, Guid submitterId);
    }
}