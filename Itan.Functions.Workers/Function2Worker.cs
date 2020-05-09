using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Itan.Functions.Models;
using Itan.Functions.Workers.Model;
using Itan.Functions.Workers.Wrappers;

namespace Itan.Functions.Workers
{
    public class Function2Worker : IFunction2Worker
    {
        private readonly ILoger<Function2Worker> log;
        private readonly IChannelsDownloadsReader downloadsReader;
        private readonly IBlobContainer blobContainer;
        private readonly IBlobPathGenerator blobPathGenerator;
        private readonly IHttpDownloader httpDownloader;
        private readonly IChannelsDownloadsWriter downloadsWriter;
        private readonly ISerializer serializer;

        public Function2Worker(
            ILoger<Function2Worker> log,
            IChannelsDownloadsReader downloadsReader,
            IBlobPathGenerator blobPathGenerator,
            IHttpDownloader httpDownloader,
            IBlobContainer blobContainer,
            IChannelsDownloadsWriter downloadsWriter,
            ISerializer serializer)
        {
            Ensure.NotNull(log, nameof(log));
            Ensure.NotNull(downloadsReader, nameof(downloadsReader));
            Ensure.NotNull(blobPathGenerator, nameof(blobPathGenerator));
            Ensure.NotNull(httpDownloader, nameof(httpDownloader));
            Ensure.NotNull(blobContainer, nameof(blobContainer));
            Ensure.NotNull(downloadsWriter, nameof(downloadsWriter));
            Ensure.NotNull(serializer, nameof(serializer));

            this.log = log;
            this.downloadsReader = downloadsReader;
            this.blobPathGenerator = blobPathGenerator;
            this.httpDownloader = httpDownloader;
            this.blobContainer = blobContainer;
            this.downloadsWriter = downloadsWriter;
            this.serializer = serializer;
        }

        public async Task Run(string queueItem)
        {
            var channelToDownload = this.serializer.Deserialize<ChannelToDownload>(queueItem);

            var channelString = await this.httpDownloader.GetStringAsync(channelToDownload.Url);
            var hashCode = channelString.GetHashCode();

            if (await this.downloadsReader.Exists(channelToDownload.Id, hashCode))
            {
                return;
            }

            var channelDownloadPath = this.blobPathGenerator.GetChannelDownloadPath(channelToDownload.Id);
            await this.blobContainer.UploadStringAsync("rss",channelDownloadPath, channelString);

            var data = new DownloadDto
            {
                ChannelId = channelToDownload.Id,
                Path = channelDownloadPath,
                HashCode = hashCode
            };

            await this.downloadsWriter.InsertAsync(data);
        }
    }
}