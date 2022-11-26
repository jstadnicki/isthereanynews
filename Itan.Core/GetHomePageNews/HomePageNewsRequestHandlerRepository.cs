using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Core.GetHomePageNews
{
    class HomePageNewsRequestHandlerRepository : IHomePageNewsRequestHandlerRepository
    {
        private string _connectionString;
        private string _storage;

        public HomePageNewsRequestHandlerRepository(IOptions<ConnectionOptions> options)
        {
            _connectionString = options.Value.SqlReader;
            _storage = options.Value.Storage;
        }

        public async Task<HomePageNewsViewModel> GetHomePageNews()
        {
            var queries = new List<string>();

            for (int i = 1; i <= 7; i++)
            {
                var q = $"SELECT TOP 1 n.id, n.Title, n.Published, n.Link, c.Id as ChannelId, c.Title as Author" +
                        $" FROM News n join Channels c on n.ChannelId = c.Id" +
                        $" WHERE n.Published > @date{i} ORDER BY n.Published ASC;";
                queries.Add(q);
            }

            for (var i = 0; i < 24; i++)
            {
                var q = $"SELECT TOP 1 n.id, n.Title, n.Published, n.Link, c.Id as ChannelId, c.Title as Author" +
                        $" FROM News n join Channels c on n.ChannelId = c.Id " +
                        $" WHERE n.Published > @date24{i} ORDER BY n.Published ASC;";
                queries.Add(q);
            }


            var today = DateTime.UtcNow.Date;
            var queryData = GenerateQueryData(today);

            var query = string.Join("\n", queries);
            List<LandingPageNewsViewModel> queryResult = new List<LandingPageNewsViewModel>();
            HomePageNewsViewModel result = new HomePageNewsViewModel();

            using (var connection = new SqlConnection(_connectionString))
            {
                var reader = await connection.QueryMultipleAsync(query, queryData);
                List<LandingPageNewsViewModel> news;
                try
                {
                    for (int i = 0; i < queries.Count; i++)
                    {
                        var readAsync = await reader.ReadAsync<LandingPageNewsViewModel>();
                        news = readAsync.ToList();
                        queryResult.AddRange(news);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                for (int i = 0; i < 7; i++)
                {
                    if (result.TopNews.Select(x => x.Id).Contains(queryResult[i].Id) == false)
                    {
                        result.TopNews.Add(queryResult[i]);
                    }
                }
                
                for (int i = 7; i < 31; i++)
                {
                    if (result.BottomNews.Select(x => x.Id).Contains(queryResult[i].Id) == false)
                    {
                        result.BottomNews.Add(queryResult[i]);
                    }
                }
            }

            var blobClient = new BlobServiceClient(_storage);
            var container = blobClient.GetBlobContainerClient("rss");

            queryResult.ForEach(x =>
            {
                var itemBlobUrl = $"items/{x.ChannelId}/{x.Id.ToString()}.json";
                var blobSasBuilder = new BlobSasBuilder()
                {
                    StartsOn = DateTime.UtcNow,
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
                    BlobContainerName = "rss",
                    BlobName = itemBlobUrl,
                };
                blobSasBuilder.SetPermissions(BlobSasPermissions.Read);
                
                var blob = container.GetBlobClient(itemBlobUrl);
                x.ContentLink = blob.GenerateSasUri(blobSasBuilder).ToString();
            });

            return result;
        }

        private object GenerateQueryData(DateTime today) =>
            new 
            {
                date1 = today.AddDays(-0),
                date2 = today.AddDays(-1),
                date3 = today.AddDays(-2),
                date4 = today.AddDays(-3),
                date5 = today.AddDays(-4),
                date6 = today.AddDays(-5),
                date7 = today.AddDays(-6),
                date240 = today.AddDays(-1).AddHours(0),
                date241 = today.AddDays(-1).AddHours(1),
                date242 = today.AddDays(-1).AddHours(2),
                date243 = today.AddDays(-1).AddHours(3),
                date244 = today.AddDays(-1).AddHours(4),
                date245 = today.AddDays(-1).AddHours(5),
                date246 = today.AddDays(-1).AddHours(6),
                date247 = today.AddDays(-1).AddHours(7),
                date248 = today.AddDays(-1).AddHours(8),
                date249 = today.AddDays(-1).AddHours(9),
                date2410 = today.AddDays(-1).AddHours(10),
                date2411 = today.AddDays(-1).AddHours(11),
                date2412 = today.AddDays(-1).AddHours(12),
                date2413 = today.AddDays(-1).AddHours(13),
                date2414 = today.AddDays(-1).AddHours(14),
                date2415 = today.AddDays(-1).AddHours(15),
                date2416 = today.AddDays(-1).AddHours(16),
                date2417 = today.AddDays(-1).AddHours(17),
                date2418 = today.AddDays(-1).AddHours(18),
                date2419 = today.AddDays(-1).AddHours(19),
                date2420 = today.AddDays(-1).AddHours(20),
                date2421 = today.AddDays(-1).AddHours(21),
                date2422 = today.AddDays(-1).AddHours(22),
                date2423 = today.AddDays(-1).AddHours(23),
                date2424 = today.AddDays(-1).AddHours(24)
            };
    }
}