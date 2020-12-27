using System.IO;
using System.Threading.Tasks;

namespace Itan.Wrappers
{
    public class StreamReaderWrapper : IStreamBlobReader
    {
        public async Task<string> ReadAllAsTextAsync(Stream myBlob)
        {
            using var textReader = new StreamReader(myBlob);
            return await textReader.ReadToEndAsync();
        }
    }
}