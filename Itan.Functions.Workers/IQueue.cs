using System.Collections.Generic;
using System.Threading.Tasks;
using Itan.Functions.Models;

namespace Itan.Functions.Workers
{
    public interface IQueue<T>
    {
        Task AddRangeAsync(IEnumerable<T> elementsToAdd);
        Task AddAsync(ChannelUpdate channelUpdate, string s);
    }
}