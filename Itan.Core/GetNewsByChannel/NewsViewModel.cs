using System;
using System.Collections.Generic;

namespace Itan.Core.GetNewsByChannel
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
        public List<NewsHeaderTagViewModel> Tags { get; set; }
    }

    public class NewsHeaderTagViewModel
    {
        public Guid TagId { get; set; }
        public Guid NewsId { get; set; }
        public string Text { get; set; }
    }
}