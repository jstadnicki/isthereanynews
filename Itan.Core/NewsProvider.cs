using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Itan.Core
{
    public class NewsProvider
    {
        private readonly IConfiguration configuration;

        public NewsProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public List<NewsViewModel> GetAllByChannelId(Guid channelId)
        {
            var connectionString = "server=.;database=itan;User Id=itanreaduser;password=12qw!@QW";
            var newsHeaderList = new List<NewsHeader>();
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "select n.id,n.Title, n.Published, n.Link from News n where n.ChannelId = @channelId order by n.Published desc";
                var queryData = new
                {
                    channelId = channelId
                };

                var queryResult = connection.Query<NewsHeader>(query, queryData);
                newsHeaderList.AddRange(queryResult);
            }


            var emulatorConnectionString = this.configuration.GetConnectionString("emulator");
            var account = CloudStorageAccount.Parse(emulatorConnectionString);
            var serviceClient = account.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference("rss");

            var sharedAccessBlobPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.UtcNow,
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(1)
            };

            var itemsToDownload = newsHeaderList.Select(x =>
            {
                var itemBlobUrl = $"items/{channelId}/{x.Id.ToString()}.json";
                var blob = container.GetBlobReference(itemBlobUrl);
                var sas = blob.GetSharedAccessSignature(sharedAccessBlobPolicy);
                var newsViewModel = new NewsViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Published = x.Published,
                    ContentUrl = blob.Uri + sas,
                    Link = x.Link
                };
                return newsViewModel;
            });

            return itemsToDownload.ToList();
        }

        private class NewsHeader
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public DateTime Published { get; set; }

            public string Link { get; set; }
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

            var query24 = Create24Query();

            HomePageNews result = new HomePageNews();

            var connectionString = "server=.;database=itan;User Id=itanreaduser;password=12qw!@QW";
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

            using (var connection = new SqlConnection(connectionString))
            {
                var reader = await connection.QueryMultipleAsync(query, queryData);
                List<LandingPageNews> news;
                try
                {
                    for (int i = 0; i < 28; i++)
                    {
                        news = (await reader.ReadAsync<LandingPageNews>()).ToList();
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

            var emulatorConnectionString = this.configuration.GetConnectionString("emulator");
            var account = CloudStorageAccount.Parse(emulatorConnectionString);
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