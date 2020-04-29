using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Itan.Functions.Models;
using Itan.Functions.Workers.Wrappers;
using Moq;
using Xunit;

namespace Itan.Functions.Workers.Tests.FunctionWorker3
{
    public class Function3WorkerTests
    {
        [Fact]
        public void PassingNullToConstructorThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Function3Worker(null, null, null, null, null, null, null, null));

            Assert.Throws<ArgumentNullException>(() => new Function3Worker(
                Mock.Of<ILoger<Function3Worker>>(),
                null, null, null, null, null, null, null));

            Assert.Throws<ArgumentNullException>(() => new Function3Worker(
                Mock.Of<ILoger<Function3Worker>>(),
                Mock.Of<IStreamBlobReader>(), null, null, null, null, null, null));

            Assert.Throws<ArgumentNullException>(() => new Function3Worker(
                Mock.Of<ILoger<Function3Worker>>(),
                Mock.Of<IStreamBlobReader>(),
                Mock.Of<IFeedReader>(),
                null, null, null, null, null));

            Assert.Throws<ArgumentNullException>(() => new Function3Worker(
                Mock.Of<ILoger<Function3Worker>>(),
                Mock.Of<IStreamBlobReader>(),
                Mock.Of<IFeedReader>(),
                Mock.Of<IQueue<ChannelUpdate>>(),
                null, null, null, null));

            Assert.Throws<ArgumentNullException>(() => new Function3Worker(
                Mock.Of<ILoger<Function3Worker>>(),
                Mock.Of<IStreamBlobReader>(),
                Mock.Of<IFeedReader>(),
                Mock.Of<IQueue<ChannelUpdate>>(),
                Mock.Of<IBlobPathGenerator>(),
                null, null, null));

            Assert.Throws<ArgumentNullException>(() => new Function3Worker(
                Mock.Of<ILoger<Function3Worker>>(),
                Mock.Of<IStreamBlobReader>(),
                Mock.Of<IFeedReader>(),
                Mock.Of<IQueue<ChannelUpdate>>(),
                Mock.Of<IBlobPathGenerator>(),
                Mock.Of<IBlobContainer>(),
                null, null));

            Assert.Throws<ArgumentNullException>(() => new Function3Worker(
                Mock.Of<ILoger<Function3Worker>>(),
                Mock.Of<IStreamBlobReader>(),
                Mock.Of<IFeedReader>(),
                Mock.Of<IQueue<ChannelUpdate>>(),
                Mock.Of<IBlobPathGenerator>(),
                Mock.Of<IBlobContainer>(),
                Mock.Of<ISerializer>(),
                null));
        }

        [Theory, AutoData]
        public async Task FeederCannotParseAndThrowsWorkerHandlesAndLogsCriticalInformation(
            Function3WorkerFixture fixture, Guid channelId, string blobName)
        {
            var worker = fixture
                .MakeReaderReturnEmptyString()
                .MakeFeedWrapperThrowException()
                .GetWorker();

            await worker.RunAsync(channelId, blobName, null);

            fixture.MockLoger.Verify(v => v.LogCritical(It.IsAny<string>()), Times.Exactly(2));
        }

        [Theory, AutoData]
        public async Task FeedReturnsNewFeedWithoutItemsUpdateUpdateQueueIsTriggeredNoBlobsAreCreated(Function3WorkerFixture fixture, Guid channelId, string blobName, string title,
            string description)
        {
            var worker = fixture
                .MakeFeedReturnValidFeedWithoutItems(title, description)
                .GetWorker();

            await worker.RunAsync(channelId, blobName, null);

            fixture.MockQueue.Verify(
                v => v.AddAsync(
                    It.Is<ChannelUpdate>(p => p.Description == description && p.Title == title && p.Id == channelId),
                    It.Is<string>(p => p == QueuesName.ChannelUpdate)), Times.Once);

            fixture.MockBlobContainer.Verify(
                v => v.UploadTextAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            fixture.MockBlobContainer.Verify(v => v.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            fixture.MockNewsWriter.Verify(
                v => v.InsertNewsLinkAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>()), Times.Never);
        }
    }
}