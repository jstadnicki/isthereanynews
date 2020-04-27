using System.Threading.Tasks;

namespace Itan.Functions.Workers.Wrappers
{
    public interface IBlobContainer
    {
        Task UploadTextAsync(string containerName, string id, string channelString);
        Task DeleteAsync(string containerName, string itemUploadPath);
    }
}