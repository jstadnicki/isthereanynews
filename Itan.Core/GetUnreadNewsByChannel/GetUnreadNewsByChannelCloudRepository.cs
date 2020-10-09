using System;
using System.Collections.Generic;
using System.Linq;
using Itan.Common;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Itan.Core.GetUnreadNewsByChannel
{
    internal class GetUnreadNewsByChannelCloudRepository : IGetUnreadNewsByChannelCloudRepository
    {
        private string emulator;

        public GetUnreadNewsByChannelCloudRepository(ConnectionOptions connectionOptions)
        {
            this.emulator = connectionOptions.Emulator;
        }

        public List<NewsViewModel> GetNewsViewModel(string requestChannelId, List<NewsHeader> newsHeaders)
        {
            var account = CloudStorageAccount.Parse(this.emulator);
            var serviceClient = account.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference("rss");

            var sharedAccessBlobPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.UtcNow,
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(1)
            };

            var itemsToDownload = newsHeaders.Select(x =>
            {
                var itemBlobUrl = $"items/{requestChannelId}/{x.Id.ToString()}.json";
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
    }
}