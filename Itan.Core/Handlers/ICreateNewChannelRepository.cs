using System;
using System.Threading.Tasks;

namespace Itan.Core.Handlers
{
    public interface ICreateNewChannelRepository
    {
        Task Save(string url, Guid submitterId);
    }
}