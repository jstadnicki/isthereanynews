using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Itan.Common;
using Itan.Core.GetNewsByChannel;
using Microsoft.Extensions.Options;

namespace Itan.Core.GetUnreadNewsByChannel
{
    internal class GetUnreadNewsByChannelCloudRepository : IGetUnreadNewsByChannelCloudRepository
    {
        private string _storage;

        public GetUnreadNewsByChannelCloudRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _storage = connectionOptions.Value.Storage;
        }

        public List<NewsViewModel> GetNewsViewModel(string requestChannelId, List<NewsHeader> newsHeaders,
            List<NewsHeaderTagViewModel> newsHeaderTagViewModels)
        {
            var blobClient = new BlobServiceClient(_storage);
            var container = blobClient.GetBlobContainerClient("rss");

            
            var itemsToDownload = newsHeaders.Select(x =>
            {
                var itemBlobUrl = $"items/{requestChannelId}/{x.Id.ToString()}.json";
                var blob = container.GetBlobClient(itemBlobUrl);
                
                var blobSasBuilder = new BlobSasBuilder()
                {
                    StartsOn = DateTime.UtcNow,
                    ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
                    BlobContainerName = "rss",
                    BlobName = itemBlobUrl
                };
                
                blobSasBuilder.SetPermissions(BlobSasPermissions.Read);

                var newsViewModel = new NewsViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Published = x.Published,
                    ContentUrl = blob.GenerateSasUri(blobSasBuilder).ToString(),
                    Link = x.Link,
                    OriginalPostId = x.OriginalPostId,
                    Tags = newsHeaderTagViewModels.Where(nht => nht.NewsId == x.Id).ToList()
                };
                return newsViewModel;
            });

            return itemsToDownload.ToList();
        }
    }
}