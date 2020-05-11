using System;

namespace Itan.Functions.Workers
{
    public interface IBlobPathGenerator
    {
        string CreateChannelDownloadPath(Guid id);
        string GetChannelDownloadPath(Guid id, string blobName);
        string GetPathUpload(Guid channelId, Guid itemId);
    }
}