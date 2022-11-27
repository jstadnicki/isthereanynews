using System.Threading.Tasks;
using Itan.Common;
using Itan.Functions.Workers.Model;
using Itan.Wrappers;

namespace Itan.Functions.Workers
{
    public class Function2Worker : IFunction2Worker
    {
        private readonly ILoger<Function2Worker> _log;
        private readonly IChannelsDownloadsReader _downloadsReader;
        private readonly IBlobContainer _blobContainer;
        private readonly IBlobPathGenerator _blobPathGenerator;
        private readonly IHttpDownloader _httpDownloader;
        private readonly IChannelsDownloadsWriter _downloadsWriter;
        private readonly ISerializer _serializer;
        private readonly IHashSum _hasher;

        public Function2Worker(
            ILoger<Function2Worker> log,
            IChannelsDownloadsReader downloadsReader,
            IBlobPathGenerator blobPathGenerator,
            IHttpDownloader httpDownloader,
            IBlobContainer blobContainer,
            IChannelsDownloadsWriter downloadsWriter,
            ISerializer serializer,
            IHashSum hasher
            )
        {
            Ensure.NotNull(log, nameof(log));
            Ensure.NotNull(downloadsReader, nameof(downloadsReader));
            Ensure.NotNull(blobPathGenerator, nameof(blobPathGenerator));
            Ensure.NotNull(httpDownloader, nameof(httpDownloader));
            Ensure.NotNull(blobContainer, nameof(blobContainer));
            Ensure.NotNull(downloadsWriter, nameof(downloadsWriter));
            Ensure.NotNull(serializer, nameof(serializer));
            Ensure.NotNull(hasher, nameof(hasher));

            _log = log;
            _downloadsReader = downloadsReader;
            _blobPathGenerator = blobPathGenerator;
            _httpDownloader = httpDownloader;
            _blobContainer = blobContainer;
            _downloadsWriter = downloadsWriter;
            _serializer = serializer;
            _hasher = hasher;
        }

        public async Task Run(string queueItem)
        {
            var channelToDownload = _serializer.Deserialize<ChannelToDownload>(queueItem);

            var channelString = await _httpDownloader.GetStringAsync(channelToDownload.Url);
            if (string.IsNullOrWhiteSpace(channelString))
            {
                return;
            }

            var sha = _hasher.GetHash(channelString);

            if (await _downloadsReader.Exists(channelToDownload.Id, sha))
            {
                return;
            }

            var channelDownloadPath = _blobPathGenerator.CreateChannelDownloadPath(channelToDownload.Id);
            await _blobContainer.UploadStringAsync("rss", channelDownloadPath, channelString, IBlobContainer.UploadStringCompression.GZip);

            var data = new DownloadDto
            {
                ChannelId = channelToDownload.Id,
                Path = channelDownloadPath,
                Sha = sha
            };

            await _downloadsWriter.InsertAsync(data);
        }
    }
}