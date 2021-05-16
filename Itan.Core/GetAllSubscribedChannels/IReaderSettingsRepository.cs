using System;
using System.Threading.Tasks;
using Itan.Core.GetUnreadNewsByChannel;

namespace Itan.Core.GetAllSubscribedChannels
{
    public interface IReaderSettingsRepository
    {
        Task<ReaderSettings> GetAsync(string personId);
        Task CreateDefaultValuesAsync(Guid requestUserId);
    }
}