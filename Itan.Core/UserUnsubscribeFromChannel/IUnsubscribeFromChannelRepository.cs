using System;
using System.Threading.Tasks;

namespace Itan.Core.UserUnsubscribeFromChannel
{
    public interface IUnsubscribeFromChannelRepository
    {
        Task DeleteSubscriptionAsync(Guid channelId, Guid personId);
    }
}