using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Itan.Functions.Models;
using Itan.Functions.Workers.Model;
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
        public async Task FeedReturnsNewFeedWithoutItemsUpdateUpdateQueueIsTriggeredNoBlobsAreCreated(
            Function3WorkerFixture fixture, Guid channelId, string blobName, string title,
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
                v => v.UploadStringAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<IBlobContainer.UploadStringCompression>()), Times.Never);
            fixture.MockBlobContainer.Verify(v => v.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            fixture.MockNewsWriter.Verify(
                v => v.InsertNewsLinkAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>(),
                    It.IsAny<DateTime>()), Times.Never);
        }

        [Theory, AutoData]
        public async Task FeedWithItemsUploadEveryElementToCloudAndSimpleVersionToNewsWriter(
            Function3WorkerFixture fixture,
            Guid channelId,
            string blobName,
            string title,
            string description,
            int itemsCount,
            string uploadPath,
            string serialized)
        {
            var worker = fixture
                .MakeFeedReturnValidFeedWithItems(title, description, itemsCount)
                .MakeSerializerReturn(serialized)
                .MakePathGeneratorGenerate(uploadPath)
                .GetWorker();

            await worker.RunAsync(channelId, blobName, null);

            fixture.MockQueue.Verify(
                v => v.AddAsync(
                    It.Is<ChannelUpdate>(p => p.Description == description && p.Title == title && p.Id == channelId),
                    It.Is<string>(p => p == QueuesName.ChannelUpdate)), Times.Once);

            fixture.MockSerializer.Verify(v => v.Serialize(It.IsAny<ItanFeedItem>()), Times.Exactly(itemsCount));

            fixture.MockPathGenerator
                .Verify(v => v.GetPathUpload(It.Is<Guid>(p => p == channelId), It.IsAny<Guid>()),
                    Times.Exactly(itemsCount));

            fixture.MockBlobContainer
                .Verify(
                    v => v.UploadStringAsync(It.Is<string>(p => p == "rss"), It.Is<string>(p => p == uploadPath),
                        It.Is<string>(p => p == serialized), It.IsAny<IBlobContainer.UploadStringCompression>()),
                    Times.Exactly(itemsCount));

            fixture.MockNewsWriter
                .Verify(
                    v => v.InsertNewsLinkAsync(It.Is<Guid>(p => p == channelId), It.IsAny<string>(), It.IsAny<Guid>(),
                        It.IsAny<DateTime>()),
                    Times.Exactly(itemsCount));
        }

        [Theory, AutoData]
        public async Task WritingToNewsStorageThrowsThenNewsIsAlsoDeletedFromCloud(
            Function3WorkerFixture fixture,
            Guid channelId,
            string blobName,
            string title,
            string description,
            int itemsCount,
            string uploadPath,
            string serialized)
        {
            var worker = fixture
                .MakeFeedReturnValidFeedWithItems(title, description, itemsCount)
                .MakeSerializerReturn(serialized)
                .MakePathGeneratorGenerate(uploadPath)
                .MakeNewsWriterThrowsOnWrite()
                .GetWorker();

            await worker.RunAsync(channelId, blobName, null);

            fixture.MockQueue.Verify(
                v => v.AddAsync(
                    It.Is<ChannelUpdate>(p => p.Description == description && p.Title == title && p.Id == channelId),
                    It.Is<string>(p => p == QueuesName.ChannelUpdate)), Times.Once);

            fixture.MockSerializer.Verify(v => v.Serialize(It.IsAny<ItanFeedItem>()), Times.Exactly(itemsCount));

            fixture.MockPathGenerator
                .Verify(v => v.GetPathUpload(It.Is<Guid>(p => p == channelId), It.IsAny<Guid>()),
                    Times.Exactly(itemsCount));

            fixture.MockBlobContainer
                .Verify(
                    v => v.UploadStringAsync(It.Is<string>(p => p == "rss"), It.Is<string>(p => p == uploadPath),
                        It.Is<string>(p => p == serialized), It.IsAny<IBlobContainer.UploadStringCompression>()),
                    Times.Exactly(itemsCount));

            fixture.MockNewsWriter
                .Verify(
                    v => v.InsertNewsLinkAsync(It.Is<Guid>(p => p == channelId), It.IsAny<string>(), It.IsAny<Guid>(),
                        It.IsAny<DateTime>()),
                    Times.Exactly(itemsCount));

            fixture.MockLoger.Verify(v => v.LogCritical(It.IsAny<string>()), Times.Exactly(itemsCount));

            fixture.MockBlobContainer.Verify(v => v.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Exactly(itemsCount));
        }
    }
}