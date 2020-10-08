using System;
using System.Threading.Tasks;

namespace Itan.Core.MarkNewsRead
{
    public interface IMarkNewsReadRepository
    {
        Task MarkReadAsync(Guid channelId, Guid newsId, Guid personId);
    }
}