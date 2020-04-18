using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Itan.Functions.Workers.Tests.FunctionWorker1;
using Moq;
using Xunit;

namespace Itan.Functions.Workers.Tests.FunctionWorker2
{
    public class Function2WorkerTests
    {
        [Theory, AutoData]
        public async Task WhenDownloadsAlreadyExistsDoesNotGenereateUploadNorInsertDownload(
            Function2WorkerFixture workerFixture)
        {
            await workerFixture
                .SerializerReturnsValidObject()
                .DownloaderReturnsEmptyString()
                .DownloadsAlreadyExists()
                .GetWorker()
                .Run(string.Empty);

            workerFixture.PathGenerator
                .Verify(v => v.GetChannelDownloadPath(It.IsAny<Guid>()), Times.Never);

            workerFixture.BlobContainer
                .Verify(v => v.UploadTextAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            workerFixture.DownloadsWriter
                .Verify(v => v.InsertAsync(It.IsAny<object>()), Times.Never);
        }

        [Theory, AutoData]
        public async Task WhenDownloadIsNewThenGeneratePathAndUploadToBlob(
            Function2WorkerFixture workerFixture,
            string uploadPath,
            Guid channelGuid,
            string url,
            string downloadContent)
        {
            await workerFixture
                .SerializerReturnsValidObject(channelGuid, url)
                .DownloaderReturns(downloadContent)
                .DownloadDoesNotExits()
                .GenerateValidBlobUploadPath(uploadPath)
                .GetWorker()
                .Run(string.Empty);

            workerFixture
                .DownloadsReader
                .Verify(
                    v => v.Exists(It.Is<Guid>(p => p == channelGuid),
                        It.Is<int>(p => p == downloadContent.GetHashCode())),
                    Times.Once());

            workerFixture
                .PathGenerator
                .Verify(v => v.GetChannelDownloadPath(It.Is<Guid>(p => p == channelGuid)), Times.Once);

            workerFixture
                .BlobContainer
                .Verify(
                    v => v.UploadTextAsync(It.Is<string>(p => p == uploadPath),
                        It.Is<string>(p => p == downloadContent)), Times.Once);

            workerFixture
                .DownloadsWriter
                .Verify(v => v.InsertAsync(It.IsAny<object>()), Times.Once);
        }
    }
}