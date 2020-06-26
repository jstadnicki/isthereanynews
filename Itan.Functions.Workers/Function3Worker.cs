using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Itan.Functions.Models;
using Itan.Functions.Workers.Exceptions;
using Itan.Functions.Workers.Wrappers;
using Microsoft.Extensions.Logging;

namespace Itan.Functions.Workers
{
    public class Function3Worker : IFunction3Worker
    {
        private readonly ILoger<Function3Worker> logger;
        private readonly IStreamBlobReader reader;
        private readonly IFeedReader feedReader;
        private readonly IQueue<ChannelUpdate> queue;
        private readonly IBlobPathGenerator pathGenerator;
        private readonly IBlobContainer blobContainer;
        private readonly ISerializer serializer;
        private readonly INewsWriter newsWriter;
        private readonly IHashSum hasher;

        public Function3Worker(
            ILoger<Function3Worker> logger,
            IStreamBlobReader reader,
            IFeedReader feedReader,
            IQueue<ChannelUpdate> queue,
            IBlobPathGenerator pathGenerator,
            IBlobContainer blobContainer,
            ISerializer serializer,
            INewsWriter newsWriter,
            IHashSum hasher)
        {
            Ensure.NotNull(logger, nameof(logger));
            Ensure.NotNull(reader, nameof(reader));
            Ensure.NotNull(feedReader, nameof(feedReader));
            Ensure.NotNull(queue, nameof(queue));
            Ensure.NotNull(pathGenerator, nameof(pathGenerator));
            Ensure.NotNull(blobContainer, nameof(blobContainer));
            Ensure.NotNull(serializer, nameof(serializer));
            Ensure.NotNull(newsWriter, nameof(newsWriter));

            this.logger = logger;
            this.reader = reader;
            this.feedReader = feedReader;
            this.queue = queue;
            this.pathGenerator = pathGenerator;
            this.blobContainer = blobContainer;
            this.serializer = serializer;
            this.newsWriter = newsWriter;
            this.hasher = hasher;
        }

        public async Task RunAsync(Guid channelId, string blobName, Stream myBlob)
        {
            try
            {
                var feedPath = this.pathGenerator.GetChannelDownloadPath(channelId, blobName);
                var feedString = await this.blobContainer.ReadBlobAsStringAsync("rss", feedPath, IBlobContainer.UploadStringCompression.GZip);
                //var feedString = await this.reader.ReadAllAsTextAsync(myBlob);
                var feed = this.feedReader.GetFeed(feedString);
                var feedItems = feed.Items;
                var channelUpdate = new ChannelUpdate
                {
                    Title = feed.Title,
                    Description = feed.Description ?? string.Empty,
                    Id = channelId
                };

                await this.queue.AddAsync(channelUpdate, QueuesName.ChannelUpdate);

                foreach (var item in feedItems)
                {
                    var itemJson = this.serializer.Serialize(item);
                    var itemUploadPath = this.pathGenerator.GetPathUpload(channelId, item.Id);
                    await this.blobContainer.UploadStringAsync("rss", itemUploadPath, itemJson, IBlobContainer.UploadStringCompression.GZip);

                    var hashCode = new
                    {
                        title = item.Title.Trim(),
                        channelId
                    }.GetHashCode();

                    var hash = this.hasher.GetHash(
                        item.Content?.Trim()
                        + item.Description?.Trim()
                        + item.Title?.Trim()
                        + item.Link?.Trim());

                    if (string.IsNullOrWhiteSpace(item.Content.Trim())
                        && string.IsNullOrWhiteSpace(item.Description.Trim())
                        && string.IsNullOrWhiteSpace(item.Title.Trim())
                        && string.IsNullOrWhiteSpace(item.Link.Trim()))
                    {
                        throw new Exception("Crap - how to calculate sha for item?");
                    }

                    try
                    {
                        await this.newsWriter.InsertNewsLinkAsync(channelId, item.Title, item.Id, item.PublishingDate, item.Link, hashCode, hash);
                    }
                    catch (NewsWriterInsertNewsLinkException e)
                    {
                        this.logger.LogCritical(e.ToString());
                        await this.blobContainer.DeleteAsync("rss", itemUploadPath);
                    }
                }
            }
            catch (FeedReaderWrapperParseStringException e)
            {
                this.logger.LogCritical($"Xml rss/raw/{channelId.ToString()}/{blobName} is broken.Finishing and returning");
                this.logger.LogCritical(e.ToString());
            }
        }
    }
}