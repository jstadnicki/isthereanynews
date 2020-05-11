using System;

namespace Itan.Functions.Workers
{
    public class BlobPathGenerator : IBlobPathGenerator
    {
        public string CreateChannelDownloadPath(Guid id) => $"raw/{id}/{DateTime.UtcNow.ToString("yyyyMMddhhmmss_mmm")}.xml";
        public string GetChannelDownloadPath(Guid id, string blobName) => $"raw/{id}/{blobName}";
        public string GetPathUpload(Guid channelId, Guid itemId) => $"items/{channelId}/{itemId.ToString()}.json";
    }
}