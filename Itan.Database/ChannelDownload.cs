using System;
using System.ComponentModel.DataAnnotations;
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
        
        [ForeignKey(nameof(ChannelId))]
        public Channel Channel { get; set; }
        
        [MaxLength(64)]
        [MinLength(64)]
        [RequiredAttribute]
        public string SHA256 { get; set; }
    }
}