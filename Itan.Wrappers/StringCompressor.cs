using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace Itan.Wrappers;

public class StringCompressor : IStringCompressor
{
    public async Task<byte[]> CompressAsync(string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        using var ms = new MemoryStream(bytes);
        using var outms = new MemoryStream();
        using var compressor = new GZipStream(outms, CompressionMode.Compress, true);

        await ms.CopyToAsync(compressor);

        await compressor.FlushAsync();

        return outms.ToArray();
    }
}