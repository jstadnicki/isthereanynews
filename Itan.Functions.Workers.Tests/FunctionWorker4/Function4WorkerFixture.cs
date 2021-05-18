using AutoFixture;
using Itan.Common;
using Itan.Wrappers;
using Moq;

namespace Itan.Functions.Workers.Tests.FunctionWorker4
{
    public class Function4WorkerFixture : Fixture
    {
        private readonly Mock<ILoger<Function4Worker>> mockLoger;
        private readonly Mock<ISerializer> mockSerializer;
        private readonly Mock<IChannelUpdater> mockUpdater;

        public Function4WorkerFixture()
        {
            this.mockUpdater = new Mock<IChannelUpdater>();
            this.mockLoger = new Mock<ILoger<Function4Worker>>();
            this.mockSerializer = new Mock<ISerializer>();
        }

        public Mock<IChannelUpdater> MockUpdater => mockUpdater;

        public Function4Worker GetWorker()
        {
            return new Function4Worker(this.mockLoger.Object, this.mockSerializer.Object, this.mockUpdater.Object);
        }

        public Function4WorkerFixture MakeSerializerReturnChannelUpdate(string description, string title)
        {
            var message = this.Build<ChannelUpdate>()
                .With(x => x.Description, description)
                .With(x => x.Title, title)
                .Create();

            this.mockSerializer
                .Setup(s => s.Deserialize<ChannelUpdate>(It.IsAny<string>()))
                .Returns(message);

            return this;
        }
    }
}