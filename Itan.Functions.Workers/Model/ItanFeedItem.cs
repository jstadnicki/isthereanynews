using System;
using System.Collections.Generic;

namespace Itan.Functions.Workers.Model
{
    public class ItanFeedItem

    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string ItemId { get; set; }
        public string Link { get; set; }
        public DateTime? PublishingDate { get; set; }
        public string PublishingDateString { get; set; }
        public ICollection<string> Categories { get; set; }
        public Guid Id { get; set; }
    }
}