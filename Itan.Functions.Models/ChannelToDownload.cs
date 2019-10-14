using System;

namespace Itan.Functions.Models
{
    public class ChannelToDownload
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
    }

    public class ChannelUpdate
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public static class QueuesName
    {
        public const string ChannelToDownload = "channel-to-download";
        public const string ChannelUpdate = "channel-update";
    }
}
