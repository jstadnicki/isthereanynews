using System;
using System.Threading.Tasks;

namespace Itan.Core.Handlers
{
    public interface IUserToChannelSubscriptionsRepository
    {
        Task<int> CreateSubscriptionAsync(Guid requestChannelId, Guid requestUserId);
    }
}