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

namespace Itan.Core.GetNewsByChannel
{
    class NewsByChannelRequestHandlerRepository : INewsByChannelRequestHandlerRepository
    {
        private string _connectionString;
        private string _storage;

        public NewsByChannelRequestHandlerRepository(IOptions<ConnectionOptions> options)
        {
            _connectionString = options.Value.SqlReader;
            _storage = options.Value.Storage;
        }

        public async Task<List<NewsViewModel>> GetAllByChannel(Guid channelId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query1 = "\n select n.id,n.Title, n.Published, n.Link " +
                         "\n from News n " +
                         "\n where n.ChannelId = @channelId " +
                         "\n AND n.OriginalPostId IS NULL " +
                         "\n order by n.Published desc;\n";

            var query2 = "\n select t.Id TagId, t.Text, n.Id NewsId from Tags t" +
                         "\n join NewsTags nt" +
                         "\n     on t.Id = nt.TagId" +
                         "\n join News n" +
                         "\n     on nt.NewsId = n.Id" +
                         "\n where n.ChannelId = @channelId";

            var queryData = new
            {
                channelId
            };

            var queryResult = await connection.QueryMultipleAsync(query1 + query2, queryData);
            var newsHeaderList = queryResult.Read<NewsHeader>().ToList();
            var newsHeaderTags = queryResult.Read<NewsHeaderTagViewModel>().ToList();


            var blobClient = new BlobServiceClient(_storage);
            var container = blobClient.GetBlobContainerClient("rss");

            var itemsToDownload = newsHeaderList.Select(x =>
            {
                var itemBlobUrl = $"items/{channelId}/{x.Id.ToString()}.json";
                var blob = container.GetBlobClient(itemBlobUrl);
                var blobSasBuilder = new BlobSasBuilder()
                {
                    StartsOn = DateTime.UtcNow,
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
                    BlobContainerName = "rss",
                    BlobName = itemBlobUrl
                };
                blobSasBuilder.SetPermissions(BlobAccountSasPermissions.Read);

                var newsViewModel = new NewsViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Published = x.Published,
                    ContentUrl = blob.GenerateSasUri(blobSasBuilder).ToString(),
                    Link = x.Link,
                    Tags = newsHeaderTags.Where(nht => nht.NewsId == x.Id).ToList()
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
    }
}