using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Itan.Core.Requests
{
    class HomePageNewsRequestHandlerRepository : IHomePageNewsRequestHandlerRepository
    {
        private string connectionString;
        private string emulator;

        public HomePageNewsRequestHandlerRepository(ConnectionOptions options)
        {
            this.connectionString = options.SqlReader;
            this.emulator = options.Emulator;
        }
        public async Task<HomePageNews> GetHomePageNews()
        {
            var date = DateTime.UtcNow.Date.AddDays(-14);

            var date1 = date;
            var date2 = date1.AddDays(-1);
            var date3 = date1.AddDays(-7);

            var query1 = "SELECT TOP 1 n.id, n.Title, n.Published, n.Link, c.Id as ChannelId, c.Title as Author FROM News n join Channels c on n.ChannelId = c.Id WHERE n.Published > @date1 ORDER BY n.Published ASC;";
            var query2 = "SELECT TOP 1 n.id, n.Title, n.Published, n.Link, c.Id as ChannelId, c.Title as Author FROM News n join Channels c on n.ChannelId = c.Id WHERE n.Published > @date2 ORDER BY n.Published ASC;";
            var query3 = "SELECT TOP 1 n.id, n.Title, n.Published, n.Link, c.Id as ChannelId, c.Title as Author FROM News n join Channels c on n.ChannelId = c.Id WHERE n.Published > @date3 ORDER BY n.Published ASC;";

            var date24start = date.AddDays(-1);

            var query24 = this.Create24Query();

            HomePageNews result = new HomePageNews();

            var queryData = new
            {
                date1,
                date2,
                date3,
                date240 = date24start.AddHours(0),
                date241 = date24start.AddHours(1),
                date242 = date24start.AddHours(2),
                date243 = date24start.AddHours(3),
                date244 = date24start.AddHours(4),
                date245 = date24start.AddHours(5),
                date246 = date24start.AddHours(6),
                date247 = date24start.AddHours(7),
                date248 = date24start.AddHours(8),
                date249 = date24start.AddHours(9),
                date2410 = date24start.AddHours(10),
                date2411 = date24start.AddHours(11),
                date2412 = date24start.AddHours(12),
                date2413 = date24start.AddHours(13),
                date2414 = date24start.AddHours(14),
                date2415 = date24start.AddHours(15),
                date2416 = date24start.AddHours(16),
                date2417 = date24start.AddHours(17),
                date2418 = date24start.AddHours(18),
                date2419 = date24start.AddHours(19),
                date2420 = date24start.AddHours(20),
                date2421 = date24start.AddHours(21),
                date2422 = date24start.AddHours(22),
                date2423 = date24start.AddHours(23),
                date2424 = date24start.AddHours(24)
            };

            var query = query1 + query2 + query3 + query24;
            List<LandingPageNews> queryResult = new List<LandingPageNews>();

            using (var connection = new SqlConnection(this.connectionString))
            {
                var reader = await connection.QueryMultipleAsync(query, queryData);
                List<LandingPageNews> news;
                try
                {
                    for (int i = 0; i < 27; i++)
                    {
                        var readAsync = await reader.ReadAsync<LandingPageNews>();
                        news = readAsync.ToList();
                        queryResult.AddRange(news);
                    }
                }
                catch (Exception e) 
                {
                    Console.WriteLine(e);
                }

                result.TopNews = queryResult.Take(3).ToList();
                result.BottomNews = queryResult.Skip(3).ToList();
            }

            var account = CloudStorageAccount.Parse(this.emulator);
            var serviceClient = account.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference("rss");

            var sharedAccessBlobPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.UtcNow,
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(1)
            };


            queryResult.ForEach(x =>
            {
                var itemBlobUrl = $"items/{x.ChannelId}/{x.Id.ToString()}.json";
                var blob = container.GetBlobReference(itemBlobUrl);
                var sas = blob.GetSharedAccessSignature(sharedAccessBlobPolicy);
                x.ContentLink = blob.Uri + sas;
            });

            return result;
        }
        private string Create24Query()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 24; i++)
            {
                var query = $"SELECT TOP 1 n.id, n.Title, n.Published, n.Link, c.Id as ChannelId, c.Title as Author FROM News n join Channels c on n.ChannelId = c.Id WHERE n.Published > @date24{i} ORDER BY n.Published ASC;";
                sb.Append(query);
            }

            return sb.ToString();
        }
    }
}