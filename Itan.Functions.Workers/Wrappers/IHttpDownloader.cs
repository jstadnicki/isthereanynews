using System.Threading.Tasks;

namespace Itan.Functions.Workers.Wrappers
{
    public interface IHttpDownloader
    {
        Task<string> GetStringAsync(string url);
    }
}