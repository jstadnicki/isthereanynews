using System;
using System.IO;
using CodeHollow.FeedReader;
using Itan.Common;
using Itan.Functions.Models;
using Itan.Functions.Workers;
using Itan.Wrappers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace Worker2Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            // var options = new Mock<IOptions<ConnectionOptions>>();
            // var jsonWrapperSerializer = new JsonWrapperSerializer();
            //
            // var worker = new Function2Worker(
            //     new Loger<Function2Worker>(new NullLogger<Function2Worker>()),
            //     Mock.Of<IChannelsDownloadsReader>(),
            //     Mock.Of<IBlobPathGenerator>(),
            //     new HttpDownloader(new Loger<HttpDownloader>(new NullLogger<HttpDownloader>())),
            //     Mock.Of<IBlobContainer>(),
            //     Mock.Of<IChannelsDownloadsWriter>(),
            //     jsonWrapperSerializer,
            //     Mock.Of<IHashSum>());
            //
            // ChannelToDownload ctd = new ChannelToDownload
            // {
            //     Id = Guid.NewGuid(),
            //     Url = "http://feeds.jeffreypalermo.com/jeffreypalermo"
            // };
            // var s = jsonWrapperSerializer.Serialize(ctd);
            // worker.Run(s).GetAwaiter().GetResult();
            // Console.ReadLine();

            //
            // FeedReaderWrapper frw = new FeedReaderWrapper();
            // var feedTest = File.ReadAllText("C:\\_temp\\test.txt");
            // var itanFeed = frw.GetFeed(feedTest);
            //
            // ChannelUpdate messsage = new ChannelUpdate
            // {
            //     Description = itanFeed.Description,
            //     Id = Guid.Parse("f2938202-9a6e-4b1f-9155-7c27262e53ca"),
            //     Title = itanFeed.Title
            // };

        }
    }
}