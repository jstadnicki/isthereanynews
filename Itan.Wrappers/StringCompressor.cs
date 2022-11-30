using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace Itan.Wrappers;

public class StringCompressor : IStringCompressor
{
    public async Task<byte[]> CompressAsync2(string text)
    {
        var uncompressedBytes = Encoding.UTF8.GetBytes(text);
        using var inputMemoryStream = new MemoryStream(uncompressedBytes);
        using var outputMemoryStream = new MemoryStream();
        using var gZipStream = new GZipStream(outputMemoryStream, CompressionMode.Compress, true);

        await inputMemoryStream.CopyToAsync(gZipStream);
        await gZipStream.FlushAsync();

        return outputMemoryStream.ToArray();
    }

    public async Task<byte[]> CompressAsync(string text)
    {
        var buffer = Encoding.UTF8.GetBytes(text);
        using var memoryStream = new MemoryStream();

        var lengthBytes = BitConverter.GetBytes((int)buffer.Length);
        memoryStream.Write(lengthBytes, 0, lengthBytes.Length);

        using var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress);
    
        gZipStream.Write(buffer, 0, buffer.Length);
        gZipStream.Flush();

        var gZipBuffer = memoryStream.ToArray();

        return gZipBuffer;
    }
}