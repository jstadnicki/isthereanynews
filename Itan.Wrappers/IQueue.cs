using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itan.Wrappers
{
    public interface IQueue
    {
        Task AddRangeAsync<T>(IEnumerable<T> elementsToAdd, string channelName);
        Task AddAsync<T>(T channelUpdate, string channelName);
    }
}