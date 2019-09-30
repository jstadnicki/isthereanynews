using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Itan.Database
{
    [Table("ChannelDownloads")]
    internal class ChannelDownload
    {
        public Guid Id { get; set; }
        public Guid ChannelId { get; set; }
        public string Path { get; set; }
        public DateTime CreatedOn { get; set; }
        public Channel Channel { get; set; }
        public int HashCode { get; set; }
    }
}