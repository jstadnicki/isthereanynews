using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;

namespace Itan.Functions.Workers.Wrappers
{
    public class BlobContainer : IBlobContainer
    {
        private readonly string emulatorConnectionString;

        public BlobContainer(IOptions<ConnectionOptions> connectionOptions)
        {
            Ensure.NotNull(connectionOptions, nameof(connectionOptions));

            this.emulatorConnectionString = connectionOptions.Value.Emulator;
        }

        public async Task UploadStringAsync(string containerName, string path, string channelString)
        {
            var account = CloudStorageAccount.Parse(this.emulatorConnectionString);
            var serviceClient = account.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            var blob = container.GetBlockBlobReference(path);
            await blob.UploadTextAsync(channelString);
        }

        public Task DeleteAsync(string containerName, string path)
        {
            var account = CloudStorageAccount.Parse(this.emulatorConnectionString);
            var serviceClient = account.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(path);
            return blob.DeleteIfExistsAsync();
        }
    }
}