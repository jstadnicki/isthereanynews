using System;
using AutoFixture.Xunit2;
using Itan.Functions.Models;
using Itan.Wrappers;
using Moq;
using Xunit;

namespace Itan.Functions.Workers.Tests.FunctionWorker4
{
    public class Function4WorkerTests
    {
        [Theory, AutoData]
        public void SerializerReturnUpdateObjectWhichIsUsedToUpdateChannelInformation(Function4WorkerFixture fixture, string item, string description, string title)
        {
            var worker = fixture
                .MakeSerializerReturnChannelUpdate(description, title)
                .GetWorker();

            worker.RunAsync(item);

            fixture.MockUpdater.Verify(
                v => v.Update(It.Is<ChannelUpdate>(p => p.Description == description && p.Title == title)), Times.Once);
        }

        [Fact]
        public void EnsureNotNullTests()
        {
            Assert.Throws<ArgumentNullException>(() => new Function4Worker(null, null, null));
            Assert.Throws<ArgumentNullException>(() => new Function4Worker(Mock.Of<ILoger<Function4Worker>>(), null, null));
            Assert.Throws<ArgumentNullException>(() => new Function4Worker(Mock.Of<ILoger<Function4Worker>>(),Mock.Of<ISerializer>(), null));
        }
    }
}