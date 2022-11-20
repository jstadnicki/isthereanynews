using System.Linq;
using AutoFixture;
using Itan.Common;
using Itan.Wrappers;
using Moq;

namespace Itan.Functions.Workers.Tests
{
    public class Function1WorkerFixture : Fixture
    {
        private readonly Mock<ILoger<Function1Worker>> _mockLoger;
        private readonly Mock<IQueue<ChannelToDownload>> _mockMessages;
        private readonly Mock<IChannelsProvider> _mockChannels;

        public Function1WorkerFixture()
        {
            _mockLoger = new Mock<ILoger<Function1Worker>>();
            _mockMessages = new Mock<IQueue<ChannelToDownload>>();
            _mockChannels = new Mock<IChannelsProvider>();
        }

        public Mock<IQueue<ChannelToDownload>> QueueMock => _mockMessages;

        public Function1Worker GetWorker()
        {
            var worker = new Function1Worker(_mockLoger.Object, _mockMessages.Object, _mockChannels.Object);
            return worker;
        }

        public Function1WorkerFixture CreateChannelsToDownloads(in int numberOfChannels)
        {
            var x = Build<ChannelToDownload>()
                .CreateMany(numberOfChannels);

            _mockChannels
                .Setup(s => s.GetAllChannelsAsync())
                .ReturnsAsync(x.ToList());

            return this;

        }
    }
}