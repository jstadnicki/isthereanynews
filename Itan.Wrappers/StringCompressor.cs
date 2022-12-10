using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace Itan.Wrappers;

public class StringCompressor : IStringCompressor
{
    public byte[] CompressAsync(string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        var ms = new MemoryStream(bytes);
        var outms = new MemoryStream();
        using var compressor = new GZipStream(outms, CompressionMode.Compress);
        ms.CopyTo(compressor);
        return outms.ToArray();
    }
}