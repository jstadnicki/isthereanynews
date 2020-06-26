using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Itan.Functions.Workers.Exceptions;
using Microsoft.Extensions.Options;

namespace Itan.Functions.Workers
{
    public class NewsWriter : INewsWriter
    {
        private string sqlConnectionString;

        public NewsWriter(IOptions<ConnectionOptions> options)
        {
            this.sqlConnectionString = options.Value.SqlWriter;
        }

        public async Task InsertNewsLinkAsync(Guid channelId, string title, Guid id, DateTime publishingDate, string link, int hashCode, string hash)
        {
            var query =
                "INSERT INTO News (Id, ChannelId, Title, CreatedOn, HashCode, Published, Link, SHA256) VALUES (@id, @channelId, @title, @createdOn, @hashCode, @published, @link, @hash)";

            var data = new
            {
                id = id,
                channelId = channelId,
                title = title.Trim(),
                createdOn = DateTime.UtcNow,
                hashCode = hashCode,
                published = publishingDate,
                link = link,
                hash = hash
            };

            using var sqlConnection = new SqlConnection(this.sqlConnectionString);
            try
            {
                await sqlConnection.ExecuteAsync(query, data);
            }
            catch (Exception e)
            {
                
                throw new NewsWriterInsertNewsLinkException(e);
            }
        }
    }
}