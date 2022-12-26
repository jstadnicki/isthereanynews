using System;

namespace Itan.Common
{
    public record ChannelToDownload
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
    }
}
