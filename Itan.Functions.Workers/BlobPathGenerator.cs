using System;

namespace Itan.Functions.Workers
{
    public class BlobPathGenerator : IBlobPathGenerator
    {
        public string GetChannelDownloadPath(Guid id) => $"raw/{id}/{DateTime.UtcNow.ToString("yyyyMMddhhmmss_mmm")}.xml";

        public string GetPathUpload(Guid channelId, Guid itemId) => $"items/{channelId}/{itemId.ToString()}.json";
    }
}