using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Itan.Common;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

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

        public async Task UploadStringAsync(
            string containerName,
            string path,
            string stringToUpload,
            IBlobContainer.UploadStringCompression compression = IBlobContainer.UploadStringCompression.None)
        {
            var account = CloudStorageAccount.Parse(this.emulatorConnectionString);
            var serviceClient = account.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            var blob = container.GetBlockBlobReference(path);
            if (compression == IBlobContainer.UploadStringCompression.GZip)
            {
                await this.CompressAndUpload(stringToUpload, blob);
            }
            else
            {
                await blob.UploadTextAsync(stringToUpload);
            }
        }

        private async Task CompressAndUpload(string stringToUpload, CloudBlockBlob blob)
        {
            byte[] jsonArrayLength;
            byte[] gzipArray;
            MemoryStream gzipMemoryStream;
            GZipStream gzip;

            try
            {
                jsonArrayLength = Encoding.UTF8.GetBytes(stringToUpload);
                await using MemoryStream stream = gzipMemoryStream = new MemoryStream(jsonArrayLength);
                using GZipStream zipStream = gzip = new GZipStream(gzipMemoryStream, CompressionLevel.Optimal);
                
                await gzip.WriteAsync(jsonArrayLength, 0, jsonArrayLength.Length);
                gzipArray = gzipMemoryStream.ToArray();

                blob.Properties.ContentEncoding = "gzip";
                blob.Properties.ContentType = "application/json";
                await blob.UploadFromByteArrayAsync(gzipArray, 0, gzipArray.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task DeleteAsync(string containerName, string path)
        {
            var account = CloudStorageAccount.Parse(this.emulatorConnectionString);
            var serviceClient = account.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(path);
            return blob.DeleteIfExistsAsync();
        }

        public async Task<string> ReadBlobAsStringAsync(string containerName, string path, IBlobContainer.UploadStringCompression compression)
        {
            var account = CloudStorageAccount.Parse(this.emulatorConnectionString);
            var serviceClient = account.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(path);

            var readStream = await blob.OpenReadAsync();

            if (compression == IBlobContainer.UploadStringCompression.None)
            {
                var streamReader = new StreamReader(readStream);
                var result = await streamReader.ReadToEndAsync();
                return result;
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var gzip = new GZipStream(readStream, CompressionMode.Decompress))
                {
                    await gzip.CopyToAsync(memoryStream);
                }

                var result = Encoding.UTF8.GetString(memoryStream.ToArray());
                return result;
            }
        }
    }
}