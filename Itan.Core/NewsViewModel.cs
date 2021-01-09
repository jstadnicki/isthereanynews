using System;

namespace Itan.Core
{
    public class NewsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ContentUrl { get; set; }
        public DateTime Published { get; set; }
        public string Link { get; set; }
        public bool Read { get; set; } = false;
        public Guid? OriginalPostId { get; set; }
    }
}