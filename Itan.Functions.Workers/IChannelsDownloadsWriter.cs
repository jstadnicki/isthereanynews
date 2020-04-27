using System.Threading.Tasks;
using Itan.Functions.Workers.Model;

namespace Itan.Functions.Workers
{
    public interface IChannelsDownloadsWriter
    {
        public Task InsertAsync(DownloadDto data);
    }
}