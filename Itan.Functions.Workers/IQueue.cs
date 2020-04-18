using System.Collections.Generic;
using System.Threading.Tasks;

public interface IQueue<T>
{
    Task AddRangeAsync(IEnumerable<T> elementsToAdd);
}