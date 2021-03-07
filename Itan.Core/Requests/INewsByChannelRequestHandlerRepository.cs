using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Itan.Core.Requests
{
    public interface INewsByChannelRequestHandlerRepository
    {
        Task<List<NewsViewModel>> GetAllByChannel(Guid channelId);
    }

    class NewsByChannelRequestHandlerRepository : INewsByChannelRequestHandlerRepository
    {
        private string connectionString;
        private string emulator;

        public NewsByChannelRequestHandlerRepository(IOptions<ConnectionOptions> options)
        {
            this.connectionString = options.Value.SqlReader;
            this.emulator = options.Value.Storage;
        }
        
        public async Task<List<NewsViewModel>> GetAllByChannel(Guid channelId)
        {
            var newsHeaderList = new List<NewsHeader>();
            using (var connection = new SqlConnection(this.connectionString))
            {
                var query = "select n.id,n.Title, n.Published, n.Link from News n where n.ChannelId = @channelId AND n.OriginalPostId IS NULL order by n.Published desc";
                var queryData = new
                {
                    channelId = channelId
                };

                var queryResult = await connection.QueryAsync<NewsHeader>(query, queryData);
                newsHeaderList.AddRange(queryResult);
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
    }
}