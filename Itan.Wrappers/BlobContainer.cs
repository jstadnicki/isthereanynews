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
        private readonly IStringCompressor _stringCompressor;
        private readonly IStringDecompressor _stringDecompressor;
        private readonly string _storage;
        private readonly BlobServiceClient _blobClient;

        public BlobContainer(
            IOptions<ConnectionOptions> connectionOptions,
            IStringCompressor stringCompressor,
            IStringDecompressor stringDecompressor)
        {
            Ensure.NotNull(connectionOptions, nameof(connectionOptions));
            Ensure.NotNull(stringCompressor, nameof(stringCompressor));
            Ensure.NotNull(stringDecompressor, nameof(stringDecompressor));

            _stringCompressor = stringCompressor;
            _stringDecompressor = stringDecompressor;
            _storage = connectionOptions.Value.Storage;

            _blobClient = new BlobServiceClient(_storage);
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
            var bytes = await _stringCompressor.CompressAsync(stringToUpload);
            var headers = new BlobHttpHeaders
            {
                ContentEncoding = "gzip",
                ContentType = "application/json"
            };

            using var ms = new MemoryStream(bytes);
            await blob.UploadAsync(ms);
            await blob.SetHttpHeadersAsync(headers);
        }


        public Task DeleteAsync(string containerName, string path)
        {
            var container = _blobClient.GetBlobContainerClient(containerName);
            var blob = container.GetBlobClient(path);
            return blob.DeleteIfExistsAsync();
        }

        public async Task<string> ReadBlobAsStringAsync(string containerName, string path,
            IBlobContainer.UploadStringCompression compression)
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

            var decompressedString = await _stringDecompressor.Decompress(outputBytes);

            return decompressedString;
        }
    }
}
