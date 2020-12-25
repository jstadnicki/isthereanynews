using System;
using System.Threading.Tasks;

namespace Itan.Core.MarkNewsOpen
{
    public interface IMarkNewsOpenRepository
    {
        Task MarkNewsOpenAsync(Guid requestChannelId, Guid requestNewsId, Guid requestUserId);
    }
}