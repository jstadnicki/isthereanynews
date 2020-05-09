using System.Threading.Tasks;

namespace Itan.Functions.Workers.Wrappers
{
    public interface IBlobContainer
    {
        Task UploadStringAsync(string containerName, string id, string channelString);
        Task DeleteAsync(string containerName, string itemUploadPath);
    }
}