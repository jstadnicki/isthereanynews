using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
                var query = "select n.id,n.Title, n.Published from News n where n.ChannelId = @channelId";
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
                    ContentUrl = blob.Uri + sas
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

        }
    }
}