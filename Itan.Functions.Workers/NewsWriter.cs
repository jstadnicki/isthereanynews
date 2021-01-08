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

        public async Task InsertNewsLinkAsync(Guid channelId, string title, Guid id, DateTime publishingDate, string link, string hash)
        {
            var query = " INSERT INTO News (Id, ChannelId, Title, CreatedOn, Published, Link, SHA256, OriginalPostId)" +
                        " VALUES (@id, @channelId, @title, @createdOn, @published, @link, @hash, (select top 1 id from News n where Link = @link order by Published, CreatedOn asc))";

            var data = new
            {
                id = id,
                channelId = channelId,
                title = title.Trim(),
                createdOn = DateTime.UtcNow,
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