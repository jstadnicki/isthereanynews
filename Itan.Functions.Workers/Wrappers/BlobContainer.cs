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
            var bytes = this.Compress(stringToUpload);
            blob.Properties.ContentEncoding = "gzip";
            blob.Properties.ContentType = "application/json";
            await blob.UploadFromByteArrayAsync(bytes, 0, bytes.Length);
        }

        private byte[] Compress(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    gs.Write(bytes, 0, bytes.Length);
                }

                return mso.ToArray();
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

            var outputBytes = new byte[readStream.Length];
            await readStream.ReadAsync(outputBytes);

            var decompressedString = Decompress(outputBytes);
            return decompressedString;
        }

        public string Decompress(byte[] data)
        {
            // Read the last 4 bytes to get the length
            byte[] lengthBuffer = new byte[4];
            Array.Copy(data, data.Length - 4, lengthBuffer, 0, 4);
            int uncompressedSize = BitConverter.ToInt32(lengthBuffer, 0);

            var buffer = new byte[uncompressedSize];
            using (var ms = new MemoryStream(data))
            {
                using (var gzip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    gzip.Read(buffer, 0, uncompressedSize);
                }
            }

            return Encoding.Unicode.GetString(buffer);
        }
    }
}