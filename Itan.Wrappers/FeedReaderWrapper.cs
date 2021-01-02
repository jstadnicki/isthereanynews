using System;
using System.Collections.Generic;
using System.Linq;
using CodeHollow.FeedReader;

namespace Itan.Wrappers
{
    public class FeedReaderWrapper : IFeedReader
    {
        public ItanFeed GetFeed(string feedString)
        {
            var feed = GetFeedFromString(feedString);
            var itanFeed = new ItanFeed
            {
                Description = feed.Description,
                Title = feed.Title,
                Items = this.GetItems(feed.Items)
            };

            return itanFeed;
        }

        private static Feed GetFeedFromString(string feedString)
        {
            try
            {
                return FeedReader.ReadFromString(feedString);
            }
            catch (Exception e)
            {
                throw new FeedReaderWrapperParseStringException(e);
            }
        }

        private IEnumerable<ItanFeedItem> GetItems(ICollection<FeedItem> feedItems)
        {
            var ordered = feedItems.Where(fi => fi.PublishingDate != null);
            var withoutPublicationDate = feedItems.Except(ordered).Reverse();
            List<ItanFeedItem> list = new List<ItanFeedItem>();
            list.AddRange(ordered.Select(this.ConvertIntoItanFeedItem));
            list.AddRange(withoutPublicationDate.Select(this.ConvertIntoItanFeedItem));
            return list;
        }

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
                PublishingDate = item.PublishingDate ?? DateTime.UtcNow,
                PublishingDateString = item.PublishingDateString,
                Categories = item.Categories,
            };
    }
}