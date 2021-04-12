using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itan.Core.GetReader
{
    public interface IReaderRepository
    {
        Task<List<ReaderSubscribedChannel>> GetSubscribedChannelsAsync(string personId);
    }
}