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
            var newsHeaderList = new List<NewsHeader>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "select n.id,n.Title, n.Published, n.Link from News n where n.ChannelId = @channelId AND n.OriginalPostId IS NULL order by n.Published desc";
                var queryData = new
                {
                    channelId = channelId
                };

                var queryResult = await connection.QueryAsync<NewsHeader>(query, queryData);
                newsHeaderList.AddRange(queryResult);
            }


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
                // var sas= blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential("",""));
                

                var newsViewModel = new NewsViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Published = x.Published,
                    ContentUrl = blob.GenerateSasUri(blobSasBuilder).ToString(),
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
    }
}