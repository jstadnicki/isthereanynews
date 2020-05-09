using System.Threading.Tasks;

namespace Itan.Functions.Workers.Wrappers
{
    public interface IBlobContainer
    {
        Task UploadStringAsync(
            string containerName,
            string id,
            string channelString,
            UploadStringCompression compression = UploadStringCompression.None);

        Task DeleteAsync(string containerName, string itemUploadPath);

        public enum UploadStringCompression
        {
            None,
            GZip
        }
    }
}