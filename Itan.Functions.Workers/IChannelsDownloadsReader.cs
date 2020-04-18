using System;
using System.Threading.Tasks;

public interface IChannelsDownloadsReader
{
    Task<bool> Exists(Guid id, int hashCode);
}