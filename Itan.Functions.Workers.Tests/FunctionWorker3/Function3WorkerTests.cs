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
        public async Task T1(Function3WorkerFixture fixture, Guid channelId, string blobName)
        {
            var worker = fixture
                .MakeReaderReturnEmptyString()
                .GetWorker();

            await worker.RunAsync(channelId, blobName, null);

            fixture.MockLoger.Verify(v => v.LogCritical(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}