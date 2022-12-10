namespace Itan.Wrappers;

public interface IStringCompressor
{
    byte[] CompressAsync(string text);
}