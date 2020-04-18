using System.Linq;
using AutoFixture;
using Itan.Functions.Models;
using Moq;

namespace Itan.Functions.Workers.Tests
{
    public class Function1WorkerFixture : Fixture
    {
        private readonly Mock<ILoger> mockLoger;
        private readonly Mock<IQueue<ChannelToDownload>> mockMessages;
        private readonly Mock<IChannelsProvider> mockChannels;

        public Function1WorkerFixture()
        {
            this.mockLoger = new Mock<ILoger>();
            this.mockMessages = new Mock<IQueue<ChannelToDownload>>();
            this.mockChannels = new Mock<IChannelsProvider>();
       
        }

        public Mock<IQueue<ChannelToDownload>> QueueMock => this.mockMessages;

        public Function1Worker GetWorker()
        {
            var worker = new Function1Worker(mockLoger.Object, mockMessages.Object, mockChannels.Object);
            return worker;
        }

        public Function1WorkerFixture CreateChannelsToDownloads(in int numberOfChannels)
        {
            var x = this.Build<ChannelToDownload>()
                .CreateMany(numberOfChannels);

            this.mockChannels
                .Setup(s => s.GetAllChannelsAsync())
                .ReturnsAsync(x.ToList());

            return this;

        }
    }
}