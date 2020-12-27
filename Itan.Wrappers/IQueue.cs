using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itan.Wrappers
{
    public interface IQueue<T>
    {
        Task AddRangeAsync(IEnumerable<T> elementsToAdd, string channelName);
        Task AddAsync(T channelUpdate, string channelName);
    }
}