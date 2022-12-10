using System.Threading.Tasks;

namespace Itan.Wrappers;

public interface IStringDecompressor
{
    string Decompress(byte[] bytes);
}