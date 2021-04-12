using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itan.Core.GetAllReaders
{
    public interface IReadersRepository
    {
        Task<List<ReadersRepository.ReaderDto>> GetAllIdsAsync();
    }
}