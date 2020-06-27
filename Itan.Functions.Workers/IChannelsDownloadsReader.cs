using System;
using System.Threading.Tasks;

namespace Itan.Functions.Workers
{
    public interface IChannelsDownloadsReader
    {
        Task<bool> Exists(Guid id, string hashCode);
    }
}