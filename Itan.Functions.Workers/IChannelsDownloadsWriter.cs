using System.Threading.Tasks;

namespace Itan.Functions.Workers
{
    public interface IChannelsDownloadsWriter
    {
        public Task InsertAsync(DownloadDto data);
    }
}