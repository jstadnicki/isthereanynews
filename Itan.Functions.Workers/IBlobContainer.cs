using System;
using System.Threading.Tasks;

namespace Itan.Functions.Workers
{
    public interface IBlobContainer
    {
        Task UploadTextAsync(string containerName, string id, string channelString);
        Task DeleteAsync(string containerName, string itemUploadPath);
    }
}