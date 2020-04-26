using System;
using System.Collections.Generic;
using System.Linq;
using CodeHollow.FeedReader;

namespace Itan.Functions.Workers
{
    public interface IFeedReader
    {
        ItanFeed GetFeed(string feedString);
    }

    class FeedReaderWrapper : IFeedReader
    {
        public ItanFeed GetFeed(string feedString)
        {
            var feed = FeedReader.ReadFromString(feedString);
            var itanFeed = new ItanFeed
            {
                Description = feed.Description,
                Title = feed.Title,
                Items = this.GetItems(feed.Items)
                
            };

            return itanFeed;
        }

        private IEnumerable<ItanFeedItem> GetItems(ICollection<FeedItem> feedItems) =>
            feedItems.Select(this.ConvertIntoItanFeedItem);

        private ItanFeedItem ConvertIntoItanFeedItem(FeedItem item) =>
            new ItanFeedItem
            {
                Id = Guid.NewGuid(),
                Title = item.Title,
                Author = item.Author,
                Content = item.Content,
                Description = item.Description,
                ItemId = item.Id,
                Link = item.Link,
                PublishingDate = item.PublishingDate,
                PublishingDateString = item.PublishingDateString,
                Categories = item.Categories,
            };
    }
}