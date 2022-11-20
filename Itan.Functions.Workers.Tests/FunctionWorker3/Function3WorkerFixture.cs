using System;
using System.Collections.Generic;
using System.IO;
using AutoFixture;
using Itan.Common;
using Itan.Functions.Workers.Exceptions;
using Itan.Wrappers;
using Moq;

namespace Itan.Functions.Workers.Tests.FunctionWorker3
{
    public class Function3WorkerFixture : Fixture
    {
        private readonly Mock<ILoger<Function3Worker>> _mockLoger;
        private readonly Mock<IStreamBlobReader> _mockReader;
        private readonly Mock<IFeedReader> _mockFeedReader;
        private readonly Mock<IQueue<ChannelUpdate>> _mockQueue;
        private readonly Mock<IBlobPathGenerator> _mockBlobPathGenerator;
        private readonly Mock<IBlobContainer> _mockBlobContainer;
        private readonly Mock<ISerializer> _mockSerializer;
        private readonly Mock<INewsWriter> _mockNewsWriter;
        private readonly Mock<IHashSum> _mockHash;

        public Function3WorkerFixture()
        {
            _mockLoger = new Mock<ILoger<Function3Worker>>();
            _mockReader = new Mock<IStreamBlobReader>();
            _mockFeedReader = new Mock<IFeedReader>();
            _mockQueue = new Mock<IQueue<ChannelUpdate>>();
            _mockBlobPathGenerator = new Mock<IBlobPathGenerator>();
            _mockBlobContainer = new Mock<IBlobContainer>();
            _mockSerializer = new Mock<ISerializer>();
            _mockNewsWriter = new Mock<INewsWriter>();
            _mockHash = new Mock<IHashSum>();
        }

        public Function3Worker GetWorker()
        {
            return new Function3Worker(
                _mockLoger.Object,
                _mockReader.Object,
                _mockFeedReader.Object,
                _mockQueue.Object,
                _mockBlobPathGenerator.Object,
                _mockBlobContainer.Object,
                _mockSerializer.Object,
                _mockNewsWriter.Object,
                _mockHash.Object);
        }

        public Mock<ILoger<Function3Worker>> MockLoger => _mockLoger;
        public Mock<IQueue<ChannelUpdate>> MockQueue => _mockQueue;
        public Mock<IBlobContainer> MockBlobContainer => _mockBlobContainer;
        public Mock<INewsWriter> MockNewsWriter => _mockNewsWriter;
        public Mock<ISerializer> MockSerializer => _mockSerializer;
        public Mock<IBlobPathGenerator> MockPathGenerator => _mockBlobPathGenerator;

        public Function3WorkerFixture MakeReaderReturnEmptyString()
        {
            _mockReader
                .Setup(s => s.ReadAllAsTextAsync(It.IsAny<Stream>()))
                .ReturnsAsync(string.Empty);

            return this;
        }

        public Function3WorkerFixture MakeFeedReturnValidFeedWithoutItems(string title, string description)
        {
            var feed = Build<ItanFeed>()
                .With(x => x.Description, description)
                .With(x => x.Title, title)
                .With(x => x.Items, new List<ItanFeedItem>())
                .Create();

            _mockFeedReader
                .Setup(s => s.GetFeed(It.IsAny<string>()))
                .Returns(feed);

            return this;
        }

        public Function3WorkerFixture MakeFeedWrapperThrowException()
        {
            _mockFeedReader.Setup(s => s.GetFeed(It.IsAny<string>()))
                .Throws(new FeedReaderWrapperParseStringException(new Exception("test one")));

            return this;
        }

        public Function3WorkerFixture MakeFeedReturnValidFeedWithItems(string title, string description, int itemsCount)
        {
            var feed = Build<ItanFeed>()
                .With(x => x.Description, description)
                .With(x => x.Title, title)
                .With(x => x.Items, this.CreateMany<ItanFeedItem>(itemsCount))
                .Create();

            _mockFeedReader
                .Setup(s => s.GetFeed(It.IsAny<string>()))
                .Returns(feed);

            return this;
        }

        public Function3WorkerFixture MakeSerializerReturn(string serialized)
        {
            _mockSerializer
                .Setup(s => s.Serialize(It.IsAny<object>()))
                .Returns(serialized);

            return this;
        }

        public Function3WorkerFixture MakePathGeneratorGenerate(string uploadPath)
        {
            _mockBlobPathGenerator
                .Setup(s => s.GetPathUpload(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(uploadPath);

            return this;
        }

        public Function3WorkerFixture MakeNewsWriterThrowsOnWrite()
        {
            MockNewsWriter
                .Setup(s => s.InsertNewsLinkAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new NewsWriterInsertNewsLinkException(new Exception()));

            return this;
        }
    }
}