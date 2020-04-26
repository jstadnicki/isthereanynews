using System.IO;
using System.Threading.Tasks;

namespace Itan.Functions.Workers
{
    public interface IStreamBlobReader
    {
        Task<string> ReadAllAsTextAsync(Stream myBlob);
    }

    public class StreamReaderWrapper : IStreamBlobReader
    {
        public async Task<string> ReadAllAsTextAsync(Stream myBlob)
        {
            using var textReader = new StreamReader(myBlob);
            return await textReader.ReadToEndAsync();
        }
    }
}