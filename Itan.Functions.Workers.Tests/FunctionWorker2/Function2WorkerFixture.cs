using System;
using AutoFixture;
using Itan.Common;
using Itan.Wrappers;
using Moq;

namespace Itan.Functions.Workers.Tests.FunctionWorker2
{
    public class Function2WorkerFixture : Fixture
    {
        private readonly Mock<IChannelsDownloadsReader> _mockChannelsDownloadsReader;
        private readonly Mock<IBlobPathGenerator> _mockBlogPathGenerator;
        private readonly Mock<IHttpDownloader> _mockHttpDownloader;
        private readonly Mock<IBlobContainer> _mockBlobContainer;
        private readonly Mock<IChannelsDownloadsWriter> _mockChannelsDownloadWriter;
        private readonly Mock<ISerializer> _mockSerializer;
        private readonly Mock<ILoger<Function2Worker>> _mockLoger;
        private IMock<IHashSum> _mockHasher;

        public Function2WorkerFixture()
        {
            _mockLoger = new Mock<ILoger<Function2Worker>>();
            _mockChannelsDownloadsReader = new Mock<IChannelsDownloadsReader>();
            _mockBlogPathGenerator = new Mock<IBlobPathGenerator>();
            _mockHttpDownloader = new Mock<IHttpDownloader>();
            _mockBlobContainer = new Mock<IBlobContainer>();
            _mockChannelsDownloadWriter = new Mock<IChannelsDownloadsWriter>();
            _mockSerializer = new Mock<ISerializer>();
            _mockHasher = new Mock<IHashSum>();
        }

        public Mock<IBlobPathGenerator> PathGenerator => _mockBlogPathGenerator;
        public Mock<IBlobContainer> BlobContainer => _mockBlobContainer;
        public Mock<IChannelsDownloadsWriter> DownloadsWriter => _mockChannelsDownloadWriter;
        public Mock<IChannelsDownloadsReader> DownloadsReader => _mockChannelsDownloadsReader;

        public Mock<IBlobPathGenerator> MockBlobPathGenerator => PathGenerator;
        public Mock<IBlobContainer> MockBlobContainer => BlobContainer;

        public Function2Worker GetWorker()
        {
            return new Function2Worker(
                _mockLoger.Object,
                _mockChannelsDownloadsReader.Object,
                _mockBlogPathGenerator.Object,
                _mockHttpDownloader.Object,
                _mockBlobContainer.Object,
                _mockChannelsDownloadWriter.Object,
                _mockSerializer.Object,
                _mockHasher.Object);
        }

        public Function2WorkerFixture SerializerReturnsValidObject()
        {
            var id = this.Create<Guid>();
            var url = this.Create<string>();

            return SerializerReturnsValidObject(id, url);
        }

        public Function2WorkerFixture SerializerReturnsValidObject(Guid channelGuid, string url)
        {
            var ctd = Build<ChannelToDownload>()
                .With(x => x.Id, () => channelGuid)
                .With(x => x.Url, () => url)
                .Create();

            _mockSerializer
                .Setup(s => s.Deserialize<ChannelToDownload>(It.IsAny<string>()))
                .Returns(ctd);

            return this;
        }

        public Function2WorkerFixture DownloaderReturnsEmptyString()
        {
            return DownloaderReturns(string.Empty);
        }

        public Function2WorkerFixture DownloadsAlreadyExists()
        {
            _mockChannelsDownloadsReader
                .Setup(s => s.Exists(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            return this;
        }

        public Function2WorkerFixture DownloadDoesNotExits()
        {
            _mockChannelsDownloadsReader
                .Setup(s => s.Exists(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            return this;
        }

        public Function2WorkerFixture GenerateValidBlobUploadPath()
        {
            var path = this.Create<string>();
            return GenerateValidBlobUploadPath(path);
        }

        public Function2WorkerFixture GenerateValidBlobUploadPath(string uploadPath)
        {
            _mockBlogPathGenerator
                .Setup(s => s.CreateChannelDownloadPath(It.IsAny<Guid>()))
                .Returns(uploadPath);

            return this;
        }

        public Function2WorkerFixture DownloaderReturns(string downloadContent)
        {
            _mockHttpDownloader
                .Setup(s => s.GetStringAsync(It.IsAny<string>()))
                .ReturnsAsync(downloadContent);

            return this;
        }

        public Function2WorkerFixture WithSerializer_ReturnsValidObject(Guid channelGuid, string url)
            => SerializerReturnsValidObject(channelGuid, url);

        public Function2WorkerFixture WithDownloader_Returns(string downloadContent)
            => DownloaderReturns(downloadContent);

        public Function2WorkerFixture WithDownloadsReader_ConfirmingExistanceOfDownload()
        {
            _mockChannelsDownloadsReader
                .Setup(s => s.Exists(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            return this;
        }
    }
}