using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itan.Core.MarkNewsRead
{
    public interface IMarkNewsReadRepository
    {
        Task MarkReadAsync(Guid channelId, Guid newsId, Guid personId,NewsReadType readType);
        Task MarkReadAsync(Guid channelId, ICollection<Guid> newsIds, Guid personId, NewsReadType readType);
        
        public enum NewsReadType
        {
            Read,
            Skip,
            Click
        }
    }
}