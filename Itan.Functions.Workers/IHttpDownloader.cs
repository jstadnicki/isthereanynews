using System.Threading.Tasks;

namespace Itan.Functions.Workers
{
    public interface IHttpDownloader
    {
        Task<string> GetStringAsync(string url);
    }
}