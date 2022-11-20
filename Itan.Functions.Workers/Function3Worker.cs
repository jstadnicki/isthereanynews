using System;
using System.IO;
using System.Threading.Tasks;
using Itan.Common;
using Itan.Functions.Workers.Exceptions;
using Itan.Wrappers;

namespace Itan.Functions.Workers
{
    public class Function3Worker : IFunction3Worker
    {
        private readonly ILoger<Function3Worker> _logger;
        private readonly IStreamBlobReader _reader;
        private readonly IFeedReader _feedReader;
        private readonly IQueue<ChannelUpdate> _queue;
        private readonly IBlobPathGenerator _pathGenerator;
        private readonly IBlobContainer _blobContainer;
        private readonly ISerializer _serializer;
        private readonly INewsWriter _newsWriter;
        private readonly IHashSum _hasher;

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

            _logger = logger;
            _reader = reader;
            _feedReader = feedReader;
            _queue = queue;
            _pathGenerator = pathGenerator;
            _blobContainer = blobContainer;
            _serializer = serializer;
            _newsWriter = newsWriter;
            _hasher = hasher;
        }

        public async Task RunAsync(Guid channelId, string blobName, Stream myBlob)
        {
            try
            {
                var feedPath = _pathGenerator.GetChannelDownloadPath(channelId, blobName);
                var feedString = await _blobContainer.ReadBlobAsStringAsync("rss", feedPath, IBlobContainer.UploadStringCompression.GZip);
                //var feedString = await this.reader.ReadAllAsTextAsync(myBlob);
                var feed = _feedReader.GetFeed(feedString);
                var feedItems = feed.Items;
                var channelUpdate = new ChannelUpdate
                {
                    Title = feed.Title,
                    Description = feed.Description ?? string.Empty,
                    Id = channelId
                };

                await _queue.AddAsync(channelUpdate, QueuesName.ChannelUpdate);

                foreach (var item in feedItems)
                {
                    var itemJson = _serializer.Serialize(item);
                    var itemUploadPath = _pathGenerator.GetPathUpload(channelId, item.Id);
                    await _blobContainer.UploadStringAsync("rss", itemUploadPath, itemJson, IBlobContainer.UploadStringCompression.GZip);

                    var hash = _hasher.GetHash(
                        item.Content?.Trim()
                        + item.Description?.Trim()
                        + item.Title?.Trim()
                        + item.Link?.Trim());

                    if (string.IsNullOrWhiteSpace(item.Content?.Trim())
                        && string.IsNullOrWhiteSpace(item.Description?.Trim())
                        && string.IsNullOrWhiteSpace(item.Title?.Trim())
                        && string.IsNullOrWhiteSpace(item.Link?.Trim()))
                    {
                        throw new Exception("Crap - how to calculate sha for item?");
                    }

                    try
                    {
                        await _newsWriter.InsertNewsLinkAsync(channelId, item.Title, item.Id, item.PublishingDate, item.Link, hash);
                    }
                    catch (NewsWriterInsertNewsLinkException e)
                    {
                        _logger.LogCritical(e.ToString());
                        await _blobContainer.DeleteAsync("rss", itemUploadPath);
                    }
                }
            }
            catch (FeedReaderWrapperParseStringException e)
            {
                _logger.LogCritical($"Xml rss/raw/{channelId.ToString()}/{blobName} is broken.Finishing and returning");
                _logger.LogCritical(e.ToString());
            }
        }
    }
}