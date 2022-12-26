using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Itan.Common;
using Itan.Functions.Workers.Exceptions;
using Itan.Wrappers;

namespace Itan.Functions.Workers
{
    public class Function3Worker : IFunction3Worker
    {
        private readonly ILoger<Function3Worker> _logger;
        private readonly IFeedReader _feedReader;
        private readonly IQueue _queue;
        private readonly IBlobPathGenerator _pathGenerator;
        private readonly IBlobContainer _blobContainer;
        private readonly ISerializer _serializer;
        private readonly INewsWriter _newsWriter;
        private readonly IHashSum _hasher;

        public Function3Worker(
            ILoger<Function3Worker> logger,
            IStreamBlobReader reader,
            IFeedReader feedReader,
            IQueue queue,
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
                var feedString =
                    await _blobContainer.ReadBlobAsStringAsync("rss", feedPath,
                        IBlobContainer.UploadStringCompression.GZip);
                var feed = _feedReader.GetFeed(feedString);
                var feedNews = feed.News;
                var channelUpdate = new ChannelUpdate
                {
                    Title = feed.Title,
                    Description = feed.Description ?? string.Empty,
                    Id = channelId
                };


                await _queue.AddAsync(channelUpdate, QueuesName.ChannelUpdate);

                foreach (var news in feedNews)
                {
                    var itemJson = _serializer.Serialize(news);
                    var itemUploadPath = _pathGenerator.GetPathUpload(channelId, news.Id);
                    await _blobContainer.UploadStringAsync("rss", itemUploadPath, itemJson,
                        IBlobContainer.UploadStringCompression.GZip);

                    var hash = _hasher.GetHash(
                        news.Content?.Trim()
                        + news.Description?.Trim()
                        + news.Title?.Trim()
                        + news.Link?.Trim());

                    if (string.IsNullOrWhiteSpace(news.Content?.Trim())
                        && string.IsNullOrWhiteSpace(news.Description?.Trim())
                        && string.IsNullOrWhiteSpace(news.Title?.Trim())
                        && string.IsNullOrWhiteSpace(news.Link?.Trim()))
                    {
                        throw new Exception("Crap - how to calculate sha for item?");
                    }

                    try
                    {
                        await _newsWriter.InsertNewsLinkAsync(channelId, news.Title, news.Id, news.PublishingDate,
                            news.Link, hash);
                        var newsCategory = new NewsCategories
                        {
                            NewsId = news.Id,
                            Categories = news.Categories.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x))
                                .ToList()
                        };
                        if (newsCategory.Categories.Any())
                        {
                            await _queue.AddAsync(newsCategory, QueuesName.NewsCategories);
                        }
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