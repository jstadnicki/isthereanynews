using System.Threading.Tasks;

public interface IChannelsDownloadsWriter
{
    public Task InsertAsync(object data);
}