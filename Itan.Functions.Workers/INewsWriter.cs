using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;

namespace Itan.Functions.Workers
{
    public interface INewsWriter
    {
        Task InsertNewsLinkAsync(Guid channelId, string title, Guid id);
    }

    class NewsWriter : INewsWriter
    {
        private string sqlConnectionString;

        public NewsWriter(IOptions<ConnectionOptions> options)
        {
            this.sqlConnectionString = options.Value.SqlWriter;
        }

        public async Task InsertNewsLinkAsync(Guid channelId, string title, Guid id)
        {
            var query =
                "INSERT INTO News (Id, ChannelId, Title, CreatedOn, HashCode) VALUES (@id, @channelId, @title, @createdOn, @hashCode)";

            var hashCode = new
            {
                title = title.Trim(),
                channelId
            }.GetHashCode();

            var data = new
            {
                id = id,
                channelId = channelId,
                title = title.Trim(),
                createdOn = DateTime.UtcNow,
                hashCode = hashCode
            };

            using var sqlConnection = new SqlConnection(this.sqlConnectionString);
            await sqlConnection.ExecuteAsync(query, data);
        }
    }
}