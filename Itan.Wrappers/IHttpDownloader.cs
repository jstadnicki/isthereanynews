using System.Threading.Tasks;

namespace Itan.Wrappers
{
    public interface IHttpDownloader
    {
        Task<string> GetStringAsync(string url);
    }
}