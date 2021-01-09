using System;

namespace Itan.Core.GetUnreadNewsByChannel
{
    public class NewsHeader
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Published { get; set; }

        public string Link { get; set; }
        public Guid? OriginalPostId { get; set; }
    }
}