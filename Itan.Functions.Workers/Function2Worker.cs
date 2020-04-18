using System;
using System.Threading.Tasks;
using Itan.Functions.Models;

namespace Itan.Functions.Workers
{
    public class Function2Worker
    {
        private readonly ILoger log;
        private readonly IChannelsDownloadsReader downloadsReader;
        private readonly IBlobContainer blobContainer;
        private readonly IBlobPathGenerator blobPathGenerator;
        private readonly IHttpDownloader httpDownloader;
        private readonly IChannelsDownloadsWriter downloadsWriter;
        private readonly ISerializer serializer;

        public Function2Worker(
            ILoger log,
            IChannelsDownloadsReader downloadsReader, 
            IBlobPathGenerator blobPathGenerator, 
            IHttpDownloader httpDownloader,
            IBlobContainer blobContainer, 
            IChannelsDownloadsWriter downloadsWriter, 
            ISerializer serializer)
        {
            this.log = log;
            this.downloadsReader = downloadsReader;
            this.blobPathGenerator = blobPathGenerator;
            this.httpDownloader = httpDownloader;
            this.blobContainer = blobContainer;
            this.downloadsWriter = downloadsWriter;
            this.serializer = serializer;
        }

        public async Task Run(string myQueueItem)
        {
            var channelToDownload = this.serializer.Deserialize<ChannelToDownload>(myQueueItem);

            var channelString = await this.httpDownloader.GetStringAsync(channelToDownload.Url);
            var hashCode = channelString.GetHashCode();

            if (await this.downloadsReader.Exists(channelToDownload.Id, hashCode))
            {
                return;
            }

            var channelDownloadPath = this.blobPathGenerator.GetChannelDownloadPath(channelToDownload.Id);
            await this.blobContainer.UploadTextAsync(channelDownloadPath, channelString);

            var data = new
            {
                id = Guid.NewGuid(),
                channelId = channelToDownload.Id,
                path = channelDownloadPath,
                createdOn = DateTime.UtcNow,
                hashCode = hashCode
            };
            
            await this.downloadsWriter.InsertAsync(data);
        }
    }
}