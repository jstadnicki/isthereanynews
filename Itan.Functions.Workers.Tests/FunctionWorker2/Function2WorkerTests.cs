using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Itan.Functions.Workers.Model;
using Itan.Wrappers;
using Moq;
using Xunit;

namespace Itan.Functions.Workers.Tests.FunctionWorker2
{
    public class Function2WorkerTests
    {
        [Theory, AutoData]
        public async Task WhenDownloadsAlreadyExistsDoesNotGenerateUploadNorInsertDownload(
            Function2WorkerFixture workerFixture)
        {
            await workerFixture
                .SerializerReturnsValidObject()
                .DownloaderReturnsEmptyString()
                .DownloadsAlreadyExists()
                .GetWorker()
                .Run(string.Empty);

            workerFixture.PathGenerator
                .Verify(v => v.CreateChannelDownloadPath(It.IsAny<Guid>()), Times.Never);

            workerFixture.BlobContainer
                .Verify(v => v.UploadStringAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IBlobContainer.UploadStringCompression>())
                    ,
                    Times.Never);

            workerFixture.DownloadsWriter
                .Verify(v => v.InsertAsync(It.IsAny<DownloadDto>()), Times.Never);
        }

        [Theory, AutoData]
        public async Task WhenDownloadIsNewThenGeneratePathAndUploadToBlob(
            Function2WorkerFixture workerFixture,
            string uploadPath,
            Guid channelGuid,
            string url,
            string downloadContent)
        {
            var worker = workerFixture
                .SerializerReturnsValidObject(channelGuid, url)
                .DownloaderReturns(downloadContent)
                .DownloadDoesNotExits()
                .GenerateValidBlobUploadPath(uploadPath)
                .GetWorker();


            await worker.Run(string.Empty);

            workerFixture
                .DownloadsReader
                .Verify(
                    v => v.Exists(It.Is<Guid>(p => p == channelGuid),
                        It.IsAny<string>()),
                    Times.Once());

            workerFixture
                .PathGenerator
                .Verify(v => v.CreateChannelDownloadPath(It.Is<Guid>(p => p == channelGuid)), Times.Once);

            workerFixture
                .BlobContainer
                .Verify(
                    v => v.UploadStringAsync(
                        It.Is<string>(p => p == "rss"),
                        It.Is<string>(p => p == uploadPath),
                        It.Is<string>(p => p == downloadContent),
                        It.IsAny<IBlobContainer.UploadStringCompression>()), Times.Once);

            workerFixture
                .DownloadsWriter
                .Verify(
                    v => v.InsertAsync(It.Is<DownloadDto>(p =>
                        p.Path == uploadPath && p.ChannelId == channelGuid)), Times.Once);
        }

        [Theory]
        [AutoData]
        public void WhenDownloaderConfirmsDownloadExistence_DoesNotGenerateNorUpload(Function2WorkerFixture fixture, Guid channelGuid, string url
            , string downloadContent, string queueItem)
        {
            // arrange
            var sut = fixture
                .WithSerializer_ReturnsValidObject(channelGuid, url)
                .WithDownloader_Returns(downloadContent)
                .WithDownloadsReader_ConfirmingExistanceOfDownload()
                .GetWorker();

            // act
            sut.Run(queueItem);

            // assert
            fixture.MockBlobPathGenerator
                .Verify(v => v.CreateChannelDownloadPath(It.IsAny<Guid>()),
                    Times.Never);

            fixture.MockBlobContainer
                .Verify(v => v.UploadStringAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IBlobContainer.UploadStringCompression>())
                    ,
                    Times.Never);
        }

        [Fact]
        public void PassingNullToConstructorThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new Function2Worker(null, null, null, null, null, null, null, null));

            Assert.Throws<ArgumentNullException>(() =>
                new Function2Worker(Mock.Of<ILoger<Function2Worker>>(), null, null, null, null, null, null, null));

            Assert.Throws<ArgumentNullException>(() =>
                new Function2Worker(Mock.Of<ILoger<Function2Worker>>(), Mock.Of<IChannelsDownloadsReader>(), null, null, null, null, null, null));

            Assert.Throws<ArgumentNullException>(() => new Function2Worker(Mock.Of<ILoger<Function2Worker>>(), Mock.Of<IChannelsDownloadsReader>()
                , Mock.Of<IBlobPathGenerator>(), null, null, null, null, null));

            Assert.Throws<ArgumentNullException>(() => new Function2Worker(Mock.Of<ILoger<Function2Worker>>(), Mock.Of<IChannelsDownloadsReader>()
                , Mock.Of<IBlobPathGenerator>(), Mock.Of<IHttpDownloader>(), null, null, null, null));

            Assert.Throws<ArgumentNullException>(() => new Function2Worker(Mock.Of<ILoger<Function2Worker>>(), Mock.Of<IChannelsDownloadsReader>()
                , Mock.Of<IBlobPathGenerator>(), Mock.Of<IHttpDownloader>(), Mock.Of<IBlobContainer>(), null, null, null));

            Assert.Throws<ArgumentNullException>(() => new Function2Worker(Mock.Of<ILoger<Function2Worker>>(), Mock.Of<IChannelsDownloadsReader>()
                , Mock.Of<IBlobPathGenerator>(), Mock.Of<IHttpDownloader>(), Mock.Of<IBlobContainer>(), Mock.Of<IChannelsDownloadsWriter>(),
                null, null));

            Assert.Throws<ArgumentNullException>(() => new Function2Worker(Mock.Of<ILoger<Function2Worker>>(), Mock.Of<IChannelsDownloadsReader>()
                , Mock.Of<IBlobPathGenerator>(), Mock.Of<IHttpDownloader>(),
                Mock.Of<IBlobContainer>(), Mock.Of<IChannelsDownloadsWriter>(), Mock.Of<ISerializer>(), null));
        }
    }
}
