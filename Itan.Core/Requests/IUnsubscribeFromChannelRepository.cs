using System;
using System.Threading.Tasks;

namespace Itan.Core.Requests
{
    public interface IUnsubscribeFromChannelRepository
    {
        Task DeleteSubscriptionAsync(Guid channelId, Guid personId);
    }
}