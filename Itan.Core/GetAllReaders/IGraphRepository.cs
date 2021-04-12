using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itan.Core.GetAllReaders
{
    public interface IGraphRepository
    {
        Task<List<GraphUserDisplayName>> GetUsersDisplayNameAsync(List<string> _);
    }
}