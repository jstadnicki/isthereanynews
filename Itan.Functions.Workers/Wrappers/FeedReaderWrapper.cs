using System;
using System.Collections.Generic;
using System.Linq;
using CodeHollow.FeedReader;
using Itan.Functions.Workers.Exceptions;
using Itan.Functions.Workers.Model;

namespace Itan.Functions.Workers.Wrappers
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