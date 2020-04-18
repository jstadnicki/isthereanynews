using System;
using AutoFixture;
using Itan.Functions.Models;
using Moq;

namespace Itan.Functions.Workers.Tests.FunctionWorker1
{
    public class Function2WorkerFixture : Fixture
    {
        private readonly Mock<ILoger> mockLoger;
        private readonly Mock<IChannelsDownloadsReader> mockChannelsDownloadsReader;
        private readonly Mock<IBlobPathGenerator> mockBlogPathGenerator;
        private readonly Mock<IHttpDownloader> mockHttpDownloader;
        private readonly Mock<IBlobContainer> mockBlobContainer;
        private readonly Mock<IChannelsDownloadsWriter> mockChannelsDownloadWriter;
        private readonly Mock<ISerializer> mockSerializer;

        public Function2WorkerFixture()
        {
            this.mockLoger = new Mock<ILoger>();
            this.mockChannelsDownloadsReader = new Mock<IChannelsDownloadsReader>();
            this.mockBlogPathGenerator = new Mock<IBlobPathGenerator>();
            this.mockHttpDownloader = new Mock<IHttpDownloader>();
            this.mockBlobContainer = new Mock<IBlobContainer>();
            this.mockChannelsDownloadWriter = new Mock<IChannelsDownloadsWriter>();
            this.mockSerializer = new Mock<ISerializer>();
        }

        public Mock<IBlobPathGenerator> PathGenerator => this.mockBlogPathGenerator;
        public Mock<IBlobContainer> BlobContainer => this.mockBlobContainer;
        public Mock<IChannelsDownloadsWriter> DownloadsWriter => this.mockChannelsDownloadWriter;
        public Mock<IChannelsDownloadsReader> DownloadsReader => this.mockChannelsDownloadsReader;

        public Function2Worker GetWorker()
        {
            return new Function2Worker(
                this.mockLoger.Object,
                this.mockChannelsDownloadsReader.Object,
                this.mockBlogPathGenerator.Object,
                this.mockHttpDownloader.Object,
                this.mockBlobContainer.Object,
                this.mockChannelsDownloadWriter.Object,
                this.mockSerializer.Object);
        }

        public Function2WorkerFixture SerializerReturnsValidObject()
        {
            var id = this.Create<Guid>();
            var url = this.Create<string>();

            return this.SerializerReturnsValidObject(id, url);
        }

        public Function2WorkerFixture SerializerReturnsValidObject(Guid channelGuid, string url)
        {
            var ctd = this.Build<ChannelToDownload>()
                .With(x => x.Id, () => channelGuid)
                .With(x => x.Url, () => url)
                .Create();

            this.mockSerializer
                .Setup(s => s.Deserialize<ChannelToDownload>(It.IsAny<string>()))
                .Returns(ctd);

            return this;
        }

        public Function2WorkerFixture DownloaderReturnsEmptyString()
        {
            return this.DownloaderReturns(string.Empty);
        }

        public Function2WorkerFixture DownloadsAlreadyExists()
        {
            this.mockChannelsDownloadsReader
                .Setup(s => s.Exists(It.IsAny<Guid>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            return this;
        }

        public Function2WorkerFixture DownloadDoesNotExits()
        {
            this.mockChannelsDownloadsReader
                .Setup(s => s.Exists(It.IsAny<Guid>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            return this;
        }

        public Function2WorkerFixture GenerateValidBlobUploadPath()
        {
            var path = this.Create<string>();
            return this.GenerateValidBlobUploadPath(path);
        }

        public Function2WorkerFixture GenerateValidBlobUploadPath(string uploadPath)
        {
            this.mockBlogPathGenerator
                .Setup(s => s.GetChannelDownloadPath(It.IsAny<Guid>()))
                .Returns(uploadPath);

            return this;
        }

        public Function2WorkerFixture DownloaderReturns(string downloadContent)
        {
            this.mockHttpDownloader
                .Setup(s => s.GetStringAsync(It.IsAny<string>()))
                .ReturnsAsync(downloadContent);

            return this;
        }
    }
}