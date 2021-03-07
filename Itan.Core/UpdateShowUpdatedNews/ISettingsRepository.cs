using System;
using System.Threading.Tasks;
using Itan.Common;

namespace Itan.Core.UpdateShowUpdatedNews
{
    public interface ISettingsRepository
    {
        public Task UpdateShowUpdatedNews(Guid userId, UpdatedNews requestShowUpdatedNews);
        Task UpdateSquashNewsUpdates(Guid userId, SquashUpdate squashNewsUpdates);
    }
}