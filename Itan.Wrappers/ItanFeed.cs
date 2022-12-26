using System.Collections.Generic;

namespace Itan.Wrappers
{
    public class ItanFeed
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<ItanFeedItem> News { get; set; }
    }
}