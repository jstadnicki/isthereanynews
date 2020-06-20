using System;

namespace Itan.Core
{
    public class LandingPageNews
    {
        public string Author { get; set; }
        public Guid ChannelId { get; set; }
        public string Title { get; set; }
        public Guid Id { get; set; }
        public DateTime Published { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public string ContentLink { get; set; }
    }
}