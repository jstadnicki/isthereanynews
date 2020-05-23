using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Itan.Database
{
    [Table("ChannelsPersons")]
    internal class ChannelsPersons
    {
        public Guid Id { get; set; }
        public Guid ChannelId { get; set; }
        public Guid PersonId { get; set; }
    
        [ForeignKey(nameof(PersonId))]
        public Person Reader { get; set; }
        
        [ForeignKey(nameof(ChannelId))]
        public Channel Channel { get; set; }
    
        public DateTime CreatedOn { get; set; }
    }
}