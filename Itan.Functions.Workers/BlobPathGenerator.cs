using System;

namespace Itan.Functions.Workers
{
    public class BlobPathGenerator : IBlobPathGenerator
    {
        public string GetChannelDownloadPath(Guid id)
        {
            var channelDownloadPath =  $"raw/{id}/{DateTime.UtcNow.ToString("yyyyMMddhhmmss_mmm")}.xml";
            return channelDownloadPath;
        }
    }
}