using System.Threading.Tasks;

namespace Itan.Wrappers
{
    public interface IBlobContainer
    {
        Task UploadStringAsync(
            string containerName,
            string id,
            string stringToUpload,
            UploadStringCompression compression = UploadStringCompression.None);

        Task DeleteAsync(string containerName, string itemUploadPath);

        Task<string> ReadBlobAsStringAsync(string containerName, string path, UploadStringCompression compression);

        public enum UploadStringCompression
        {
            None,
            GZip
        }
    }
}