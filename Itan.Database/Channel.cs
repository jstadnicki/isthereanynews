using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Itan.Database
{
    internal class Channel
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public List<ChannelDownload> Downloads { get; set; }
        public virtual List<News> News { get; set; }
        public virtual List<ChannelsPersons> PersonSubscribers { get; set; }
        public virtual ChannelSubmitter Submitter { get; set; }
        public virtual IEnumerable<ChannelNewsRead> ChannelNewsRead { get; set; }
        public virtual IEnumerable<ChannelNewsOpened> ChannelNewsOpened { get; set; }
    }
}
