using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;

namespace Itan.Functions.Workers
{
    public class BlobContainer : IBlobContainer
    {
        private string emulatorConnectionString;

        public BlobContainer(string emulatorConnectionString)
        {
            this.emulatorConnectionString = emulatorConnectionString;
        }

        public async Task UploadTextAsync(string path, string channelString)
        {
            var account = CloudStorageAccount.Parse(this.emulatorConnectionString);
            var serviceClient = account.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference("rss");
            await container.CreateIfNotExistsAsync();
            var blob = container.GetBlockBlobReference(path);
            await blob.UploadTextAsync(channelString);
        }
    }
}