using System.Collections.Generic;

namespace Itan.Functions.Workers.Model
{
    public class ItanFeed
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<ItanFeedItem> Items { get; set; }
    }
}