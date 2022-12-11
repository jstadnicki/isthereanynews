using System.Threading.Tasks;

namespace Itan.Wrappers;

public interface IStringDecompressor
{
    Task<string> Decompress(byte[] bytes);
}