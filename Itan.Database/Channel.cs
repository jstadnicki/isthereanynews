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
        public List<News> News { get; set; }
    }
}
