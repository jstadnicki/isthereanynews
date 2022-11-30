using System.Threading.Tasks;

namespace Itan.Wrappers;

public interface IStringDecompressor
{
    Task<string> DecompressAsync(byte[] bytes);
}