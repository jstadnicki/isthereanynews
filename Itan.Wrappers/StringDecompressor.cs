using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Itan.Wrappers;

public class StringDecompressor : IStringDecompressor
{
    public async Task<string> Decompress(byte[] bytes)
    {
        var inputStream = new MemoryStream(bytes);
        using var decompress = new GZipStream(inputStream, CompressionMode.Decompress);

        using var outputStream = new MemoryStream();
        await decompress.CopyToAsync(outputStream);

        outputStream.Position = 0;
        var ob = outputStream.ToArray();
        var str = Encoding.UTF8.GetString(ob);
        return str;
    }
}