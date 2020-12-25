using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Itan.Database
{
    [Table("ChannelsSubmitters")]
    internal class ChannelSubmitter
    {
        public Guid Id { get; set; }
        public Guid ChannelId { get; set; }
        public Guid PersonId { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual Person Person { get; set; }
        public virtual Channel Channel { get; set; }
    }
}