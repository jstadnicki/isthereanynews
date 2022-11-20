using System;
using System.Collections.Generic;
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
        [Required]
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime Published { get; set; }

        [Required]
        public string Link { get; set; }

        public Guid? OriginalPostId { get; set; }
        
        [MaxLength(64)]
        [MinLength(64)]
        [Required]
        public string Sha256 { get; set; }

        public virtual IEnumerable<ChannelNewsRead> ChannelNewsRead { get; set; }
        public virtual IEnumerable<ChannelNewsOpened> ChannelNewsOpened { get; set; }
    }
}