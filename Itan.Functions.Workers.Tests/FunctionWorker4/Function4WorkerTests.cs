using AutoFixture.Xunit2;
using Itan.Functions.Models;
using Moq;
using Xunit;

namespace Itan.Functions.Workers.Tests.FunctionWorker4
{
    public class Function4WorkerTests
    {
        [Theory, AutoData]
        public void SerialierReturnUpdateObjectWhichIsUsedToUpdateChannelInformation(Function4WorkerFixture fixture, string item, string description, string title)
        {
            var worker = fixture
                .MakeSerializerReturnChannelUpdate(description, title)
                .GetWorker();

            worker.RunAsync(item);

            fixture.MockUpdater.Verify(
                v => v.Update(It.Is<ChannelUpdate>(p => p.Description == description && p.Title == title)), Times.Once);
        }
    }
}