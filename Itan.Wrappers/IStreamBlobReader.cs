using System.IO;
using System.Threading.Tasks;

namespace Itan.Wrappers
{
    public interface IStreamBlobReader
    {
        Task<string> ReadAllAsTextAsync(Stream myBlob);
    }
}