using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Itan.Database
{
    [Table("ChannelsSubmitters")]
    internal class ChannelSubmitter
    {
        public Guid Id { get; set; }
        
        [ForeignKey(nameof(ChannelId))]
        public virtual Channel Channel { get; set; }
        public Guid ChannelId { get; set; }

        [ForeignKey(nameof(PersonId))]
        public virtual Person Submitter { get; set; }
        public Guid PersonId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}