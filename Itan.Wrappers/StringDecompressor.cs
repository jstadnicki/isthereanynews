using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Itan.Wrappers;

public class StringDecompressor : IStringDecompressor
{
    public async Task<string> DecompressAsync(byte[] bytes)
    {
        var outputStream = new MemoryStream();
        var inputStream = new MemoryStream(bytes);
        var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);

        await gzipStream.CopyToAsync(outputStream);

        outputStream.Position = 0;
        var streamReader = new StreamReader(outputStream);
        var decompressedString = await streamReader.ReadToEndAsync();
        return decompressedString;
    }
}