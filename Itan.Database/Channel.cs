using System;
using System.Collections.Generic;

namespace Itan.Database
{
    internal class Channel
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public List<ChannelDownload> Downloads { get; set; }
    }
}
