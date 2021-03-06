﻿using System;
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
        public virtual List<ChannelNewsRead> ChannelNewsRead { get; set; }
        public virtual List<ChannelNewsOpened> ChannelNewsOpened { get; set; } 

        public virtual List<PersonPerson> Following { get; set; }
        public virtual List<PersonPerson> Followed { get; set; }

        
        public virtual ReaderSettings Settings { get; set; }
    }
}