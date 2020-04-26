using System;

namespace Itan.Functions.Workers
{
    public interface IBlobPathGenerator
    {
        string GetChannelDownloadPath(Guid id);
        string GetPathUpload(Guid channelId, Guid itemId);
    }
}