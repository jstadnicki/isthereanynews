using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Itan.Functions.Models;
using Microsoft.Extensions.Logging;

namespace Itan.Functions.Workers
{
    public interface IFunction3Worker
    {
        Task RunAsync(Guid channelId, string blobName, Stream myBlob);
    }

    public class Function3Worker : IFunction3Worker
    {
        private readonly ILogger<Function3Worker> logger;
        private readonly IStreamBlobReader reader;
        private readonly IFeedReader feedReader;
        private readonly IQueue<ChannelUpdate> queue;
        private readonly IBlobPathGenerator pathGenerator;
        private readonly IBlobContainer blobContainer;
        private readonly ISerializer serializer;
        private readonly INewsWriter newsWriter;

        public Function3Worker(
            ILogger<Function3Worker> logger,
            IStreamBlobReader reader,
            IFeedReader feedReader,
            IQueue<ChannelUpdate> queue,
            IBlobPathGenerator pathGenerator,
            IBlobContainer blobContainer,
            ISerializer serializer,
            INewsWriter newsWriter)
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
        }

        public async Task RunAsync(Guid channelId, string blobName, Stream myBlob)
        {
            this.logger.LogInformation(
                $"Got channel with id: {channelId.ToString()} with name: {blobName} with stream length: {myBlob.Length.ToString()}");

            try
            {
                var feedString = await this.reader.ReadAllAsTextAsync(myBlob);
                var feed = this.feedReader.GetFeed(feedString);
                var feedItems = feed.Items;
                var channelUpdate = new ChannelUpdate
                {
                    Title = feed.Title,
                    Description = feed.Description,
                    Id = channelId
                };

                await this.queue.AddAsync(channelUpdate, QueuesName.ChannelUpdate);

                foreach (var item in feedItems)
                {
                    var itemJson = this.serializer.Serialize(item);
                    var itemUploadPath = this.pathGenerator.GetPathUpload(channelId, item.Id);
                    await this.blobContainer.UploadTextAsync("rss", itemUploadPath, itemJson);


                    try
                    {
                        await this.newsWriter.InsertNewsLinkAsync(channelId, item.Title, item.Id);
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
                this.logger.LogCritical(
                    $"Xml rss/raw/{channelId.ToString()}/{blobName} is broken.Finishing and returning");
                this.logger.LogCritical(e.ToString());
            }
        }
    }
}