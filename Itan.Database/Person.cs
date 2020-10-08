using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Itan.Database
{
    [Table("Persons")]
    internal class Person
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual List<ChannelsPersons> SubscribedChannels { get; set; }
        public virtual List<ChannelSubmitter> SubmittedChannels { get; set; }
        public IEnumerable<ChannelNewsRead> ChannelNewsRead { get; set; }
    }
}