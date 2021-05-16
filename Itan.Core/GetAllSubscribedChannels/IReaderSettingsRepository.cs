using System;
using System.Threading.Tasks;

namespace Itan.Core.GetUnreadNewsByChannel
{
    public interface IReaderSettingsRepository
    {
        Task<ReaderSettings> GetAsync(string personId);
        Task CreateDefaultValuesAsync(Guid requestUserId);
    }
}