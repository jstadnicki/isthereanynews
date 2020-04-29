using System;
using System.Collections.Generic;
using System.IO;
using AutoFixture;
using Itan.Functions.Models;
using Itan.Functions.Workers.Exceptions;
using Itan.Functions.Workers.Model;
using Itan.Functions.Workers.Wrappers;
using Moq;

namespace Itan.Functions.Workers.Tests.FunctionWorker3
{
    public class Function3WorkerFixture : Fixture
    {
        private readonly Mock<ILoger<Function3Worker>> mockLoger;
        private readonly Mock<IStreamBlobReader> mockReader;
        private readonly Mock<IFeedReader> mockFeedReader;
        private readonly Mock<IQueue<ChannelUpdate>> mockQueue;
        private readonly Mock<IBlobPathGenerator> mockBlobPathGenerator;
        private readonly Mock<IBlobContainer> mockBlobContainer;
        private readonly Mock<ISerializer> mockSerializer;
        private readonly Mock<INewsWriter> mockNewsWriter;

        public Function3WorkerFixture()
        {
            this.mockLoger = new Mock<ILoger<Function3Worker>>();
            this.mockReader = new Mock<IStreamBlobReader>();
            this.mockFeedReader = new Mock<IFeedReader>();
            this.mockQueue = new Mock<IQueue<ChannelUpdate>>();
            this.mockBlobPathGenerator = new Mock<IBlobPathGenerator>();
            this.mockBlobContainer = new Mock<IBlobContainer>();
            this.mockSerializer = new Mock<ISerializer>();
            this.mockNewsWriter = new Mock<INewsWriter>();
        }

        public Function3Worker GetWorker()
        {
            return new Function3Worker(
                this.mockLoger.Object,
                this.mockReader.Object,
                this.mockFeedReader.Object,
                this.mockQueue.Object,
                this.mockBlobPathGenerator.Object,
                this.mockBlobContainer.Object,
                this.mockSerializer.Object,
                this.mockNewsWriter.Object);
        }

        public Mock<ILoger<Function3Worker>> MockLoger => this.mockLoger;
        public Mock<IQueue<ChannelUpdate>> MockQueue => this.mockQueue;
        public Mock<IBlobContainer> MockBlobContainer => this.mockBlobContainer;
        public Mock<INewsWriter> MockNewsWriter => this.mockNewsWriter;

        public Function3WorkerFixture MakeReaderReturnEmptyString()
        {
            this.mockReader
                .Setup(s => s.ReadAllAsTextAsync(It.IsAny<Stream>()))
                .ReturnsAsync(string.Empty);

            return this;
        }

        public Function3WorkerFixture MakeFeedReturnValidFeedWithoutItems(string title, string description)
        {
            var feed = new ItanFeed
            {
                Description = description,
                Items = new List<ItanFeedItem>(),
                Title = title
            };

            this.mockFeedReader
                .Setup(s => s.GetFeed(It.IsAny<string>()))
                .Returns(feed);

            return this;
        }

        public Function3WorkerFixture MakeFeedWrapperThrowException()
        {
            this.mockFeedReader.Setup(s => s.GetFeed(It.IsAny<string>()))
                .Throws(new FeedReaderWrapperParseStringException(new Exception("test one")));

            return this;
        }
    }
}