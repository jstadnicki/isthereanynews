using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues;

using Itan.Common;

using Microsoft.Extensions.Options;

namespace Itan.Wrappers
{
    public class BlobContainer : IBlobContainer
    {
        private readonly string _storage;
        private readonly BlobServiceClient _blobClient;

        public BlobContainer(IOptions<ConnectionOptions> connectionOptions)
        {
            Ensure.NotNull(connectionOptions, nameof(connectionOptions));
            _storage = connectionOptions.Value.Storage;

            Uri accountUri = new Uri(_storage);
            _blobClient = new BlobServiceClient(accountUri, new DefaultAzureCredential());
        }

        public async Task UploadStringAsync(
            string containerName,
            string path,
            string stringToUpload,
            IBlobContainer.UploadStringCompression compression = IBlobContainer.UploadStringCompression.None)
        {
            var container = _blobClient.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync();
            var blob = container.GetBlobClient(path);

            if (compression == IBlobContainer.UploadStringCompression.GZip)
            {
                await CompressAndUpload(stringToUpload, blob);
            }
            else
            {
                var content = Encoding.UTF8.GetBytes(stringToUpload);
                using var ms = new MemoryStream(content);
                await blob.UploadAsync(ms);
            }
        }

        private async Task CompressAndUpload(string stringToUpload, BlobClient blob)
        {
            var bytes = Compress(stringToUpload);
            BlobHttpHeaders headers = new BlobHttpHeaders
            {
                ContentEncoding = "gzip",
                ContentType = "application/json"
            };

                using var ms = new MemoryStream(bytes);
                await blob.UploadAsync(ms);
                await blob.SetHttpHeadersAsync(headers);
        }

        private byte[] Compress(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            using var mso = new MemoryStream();
            using var gs = new GZipStream(mso, CompressionMode.Compress);
            gs.Write(bytes, 0, bytes.Length);

            return mso.ToArray();
        }

        public Task DeleteAsync(string containerName, string path)
        {
            var container = _blobClient.GetBlobContainerClient(containerName);
            var blob = container.GetBlobClient(path);
            return blob.DeleteIfExistsAsync();
        }

        public async Task<string> ReadBlobAsStringAsync(string containerName, string path, IBlobContainer.UploadStringCompression compression)
        {
            var container = _blobClient.GetBlobContainerClient(containerName);
            var blob = container.GetBlobClient(path);

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

        private string Decompress(byte[] data)
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

            return Encoding.UTF8.GetString(buffer);
        }
    }
}