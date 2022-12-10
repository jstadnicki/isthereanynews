using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Itan.Wrappers;

public class StringDecompressor : IStringDecompressor
{
    public string Decompress(byte[] bytes)
    {
        var inputStream = new MemoryStream(bytes);
        using var decompressor = new GZipStream(inputStream, CompressionMode.Decompress);

        var outputStream = new MemoryStream();
        decompressor.CopyTo(outputStream);
        var streamReader = new StreamReader(outputStream);
        var decompressedString = streamReader.ReadToEnd();
        return decompressedString;
    }
}