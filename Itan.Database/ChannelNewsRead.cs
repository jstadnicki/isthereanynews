using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Itan.Database
{
    [Table("ChannelNewsReads")]
    internal class ChannelNewsRead
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid ChannelId { get; set; }
        
        [Required]
        public Guid NewsId { get; set; }

        public DateTime CreatedOn { get; set; }
        
        [Required]
        public Guid PersonId { get; set; }

        public virtual Channel Channel { get; set; }
        public virtual News News { get; set; }
        public virtual Person Person { get; set; }
        
    }
}