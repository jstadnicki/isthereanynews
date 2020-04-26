using System;
using System.Collections.Generic;
using System.Linq;
using CodeHollow.FeedReader;
using Dapper;

namespace Itan.Functions.Workers
{
    public interface IFeedReader
    {
        ItanFeed GetFeed(string feedString);
    }

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

    internal class FeedReaderWrapperParseStringException : ItanException
    {
        public FeedReaderWrapperParseStringException(Exception exception)
            : base(nameof(FeedReaderWrapperParseStringException), exception)
        {
        }

        public override string Message => "There was a problem parsing string into CodeHollow Feed item";
    }

    public class ItanException : Exception
    {
        protected ItanException(string name, Exception exception) : base(name, exception)
        {
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}