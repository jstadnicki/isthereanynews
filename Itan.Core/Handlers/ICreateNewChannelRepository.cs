using System;
using System.Threading.Tasks;

namespace Itan.Core.Handlers
{
    public interface ICreateNewChannelRepository
    {
        Task<Guid> SaveAsync(string url, Guid submitterId);
    }
}