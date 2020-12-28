using System;
using Itan.Functions.Models;
using Itan.Functions.Workers;
using Itan.Wrappers;

namespace Worker2Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonWrapperSerializer = new JsonWrapperSerializer();
            var worker = new Function2Worker(null, null, null, new HttpDownloader(null), null, null, jsonWrapperSerializer, null);
            ChannelToDownload ctd = new ChannelToDownload
            {
                Id = Guid.NewGuid(),
                Url = "https://www.k85.pl/feed/"
            };
            var s = jsonWrapperSerializer.Serialize(ctd);
            worker.Run(s).GetAwaiter().GetResult();
            Console.ReadLine();
        }
    }
}