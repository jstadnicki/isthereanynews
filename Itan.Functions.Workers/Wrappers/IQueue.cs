using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itan.Functions.Workers.Wrappers
{
    public interface IQueue<T>
    {
        Task AddRangeAsync(IEnumerable<T> elementsToAdd, string channelName);
        Task AddAsync(T channelUpdate, string channelName);
    }
}