using System.Threading.Tasks;

namespace Itan.Wrappers;

public interface IStringCompressor
{
    Task<byte[]> CompressAsync(string text);
}