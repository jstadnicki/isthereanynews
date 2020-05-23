using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Itan.Database
{
    [Table("News")]
    internal class News
    {
        public Guid Id { get; set; }
        public Guid ChannelId { get; set; }
        [ForeignKey(nameof(ChannelId))]
        public Channel Channel { get; set; }
        public long HashCode { get; set; }
        
        [Required]
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime Published { get; set; }
    }
}