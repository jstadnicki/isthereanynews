using AutoFixture;
using Itan.Common;
using Itan.Wrappers;
using Moq;

namespace Itan.Functions.Workers.Tests.FunctionWorker4
{
    public class Function4WorkerFixture : Fixture
    {
        private readonly Mock<ILoger<Function4Worker>> _mockLoger;
        private readonly Mock<ISerializer> _mockSerializer;
        private readonly Mock<IChannelUpdater> _mockUpdater;

        public Function4WorkerFixture()
        {
            _mockUpdater = new Mock<IChannelUpdater>();
            _mockLoger = new Mock<ILoger<Function4Worker>>();
            _mockSerializer = new Mock<ISerializer>();
        }

        public Mock<IChannelUpdater> MockUpdater => _mockUpdater;

        public Function4Worker GetWorker()
        {
            return new Function4Worker(_mockLoger.Object, _mockSerializer.Object, _mockUpdater.Object);
        }

        public Function4WorkerFixture MakeSerializerReturnChannelUpdate(string description, string title)
        {
            var message = Build<ChannelUpdate>()
                .With(x => x.Description, description)
                .With(x => x.Title, title)
                .Create();

            _mockSerializer
                .Setup(s => s.Deserialize<ChannelUpdate>(It.IsAny<string>()))
                .Returns(message);

            return this;
        }
    }
}