using System.IO;
using System.Threading.Tasks;

namespace Itan.Functions.Workers.Wrappers
{
    public interface IStreamBlobReader
    {
        Task<string> ReadAllAsTextAsync(Stream myBlob);
    }
}