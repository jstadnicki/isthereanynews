using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Itan.Functions.Workers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //await UpdateChannel();
            //await UpdateNews();

            await CleanupNews();

        }

        public static async Task CleanupNews()
        {
            var connectionString = "server=.;database=itan;User Id=itanwriteuser;password=12qw!@QW";
            using var connection = new SqlConnection(connectionString);
            int counter = 1;

            while (1 == 1)
            {
                Console.WriteLine(counter++);

                var query = "select * from news where SHA256 in (select top 1 SHA256 from news group by SHA256 having count(*) > 1) ";
                var result = connection.Query<ItanFeedItemSql>(query);
                var newsToRemove = result.OrderByDescending(x => x.CreatedOn).Skip(1).ToList();
                foreach (ItanFeedItemSql itanFeedItemSql in newsToRemove)
                {
                    await DeleteNewsFromBlob(itanFeedItemSql.Id, itanFeedItemSql.ChannelId);
                    await DeleteNewsFromSql(itanFeedItemSql.Id);
                }
            }
        }

        private static async Task DeleteNewsFromSql(Guid id)
        {
            var sql = "delete from news where id=@id";
            var sqlData = new {id};
            var connectionString = "server=.;database=itan;User Id=itanwriteuser;password=12qw!@QW";
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(sql, sqlData);
        }

        public static async Task DeleteNewsFromBlob(Guid itemId, Guid channelId)
        {
            var account = CloudStorageAccount.Parse("AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;");
            var serviceClient = account.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference("rss");
            var path = $"rss/items/{channelId.ToString()}/{itemId.ToString()}.json";

            var blob = container.GetBlockBlobReference(path);

            try
            {
                await blob.DeleteIfExistsAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static async Task UpdateNews()
        {
            var sha = new SHA256Wrapper();

            int counter = 1;

            while (1 == 1)
            {
                Console.WriteLine(counter++);
                var result = GetNewsToDownload();
                if (result == null)
                {
                    break;
                }

                try
                {
                    var path = $"items/{result.ChannelId.ToString()}/{result.Id.ToString()}.json";
                    var stringContent = await ReadBlobAsStringAsync(path);
                    var item = JsonConvert.DeserializeObject<ItanFeedItemJson>(stringContent);

                    var hash = sha.GetHash(
                        item.Content?.Trim()
                        + item.Description?.Trim()
                        + item.Title?.Trim()
                        + item.Link?.Trim());

                    UpdateNewsWithSha(result.Id, hash);
                }
                catch (Exception e)
                {
                    UpdateNewsWithSha(result.Id, "Do it manually");
                }
            }
        }

        private static void UpdateNewsWithSha(Guid resultId, string hash)
        {
            var connectionString = "server=.;database=itan;User Id=itanwriteuser;password=12qw!@QW";
            using var connection = new SqlConnection(connectionString);
            var query = "update News set SHA256 = @sha where id=@id";
            var data = new
            {
                sha = hash,
                id = resultId
            };
            connection.Execute(query, data);
        }

        private static ItanFeedItemSql GetNewsToDownload()
        {
            var connectionString = "server=.;database=itan;User Id=itanreaduser;password=12qw!@QW";
            using var connection = new SqlConnection(connectionString);
            var query = "SELECT TOP 1 * FROM News WHERE SHA256 IS NULL";
            var result = connection.Query<ItanFeedItemSql>(query);
            return result.FirstOrDefault();
        }

        private static async Task UpdateChannel()
        {
            var sha = new SHA256Wrapper();
            int counter = 1;

            while (1 == 1)
            {
                Console.WriteLine(counter++);
                var result = GetChannelToDownloads();
                if (result == null)
                {
                    break;
                }

                try
                {
                    var stringContent = await ReadBlobAsStringAsync(result.Path);
                    var hash = sha.GetHash(stringContent);
                    UpdateSha(result.Id, hash);
                }
                catch (Exception e)
                {
                    UpdateSha(result.Id, "Do it manually");
                }
            }
        }

        private static void UpdateSha(Guid resultId, string hash)
        {
            var connectionString = "server=.;database=itan;User Id=itanwriteuser;password=12qw!@QW";
            using var connection = new SqlConnection(connectionString);
            var query = "update ChannelDownloads set SHA256 = @sha where id=@id";
            var data = new
            {
                sha = hash,
                id = resultId
            };
            connection.Execute(query, data);
        }

        private static ChannelDownload GetChannelToDownloads()
        {
            var connectionString = "server=.;database=itan;User Id=itanreaduser;password=12qw!@QW";
            using var connection = new SqlConnection(connectionString);
            var query = "SELECT TOP 1 * FROM ChannelDownloads WHERE SHA256 IS NULL";
            var result = connection.Query<ChannelDownload>(query);
            return result.FirstOrDefault();
        }

        public static async Task<string> ReadBlobAsStringAsync(string path)
        {
            var account = CloudStorageAccount.Parse("AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;");
            var serviceClient = account.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference("rss");
            var blob = container.GetBlockBlobReference(path);

            var readStream = await blob.OpenReadAsync();

            await using var memoryStream = new MemoryStream();
            await using var gzip = new GZipStream(readStream, CompressionMode.Decompress);
            await gzip.CopyToAsync(memoryStream);

            var result = Encoding.UTF8.GetString(memoryStream.ToArray());
            return result;
        }
    }

    internal class ChannelDownload
    {
        public Guid Id { get; set; }
        public Guid ChannelId { get; set; }
        public string Path { get; set; }
        public DateTime CreatedOn { get; set; }

        public int HashCode { get; set; }

        public string SHA256 { get; set; }
    }

    public class ItanFeedItemSql
    {
        public Guid Id { get; set; }
        public Guid ChannelId { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class ItanFeedItemJson

    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string ItemId { get; set; }
        public string Link { get; set; }
        public DateTime PublishingDate { get; set; }
        public string PublishingDateString { get; set; }
        public List<string> Categories { get; set; }
        public Guid Id { get; set; }
    }
}