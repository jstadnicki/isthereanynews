using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Core.MarkNewsOpen
{
    class MarkNewsOpenRepository : IMarkNewsOpenRepository
    {
        private readonly string connectionString;

        public MarkNewsOpenRepository(IOptions<ConnectionOptions> options)
        {
            this.connectionString = options.Value.SqlWriter;
        }

        public async Task MarkNewsOpenAsync(Guid channelId, Guid newsId, Guid personId)
        {
            var sql =
                $"if not exists (select top 1 * from ChannelNewsOpened where ChannelId=@channelId AND NewsId=@newsId AND PersonId=@personId)\n" +
                "BEGIN\n" +
                "INSERT INTO ChannelNewsOpened (Id, ChannelId, NewsId, PersonId, CreatedOn) \n" +
                "VALUES (@id, @channelId, @newsId, @personId, @createdOn)" +
                "END";

            var sqlData = new
            {
                id = Guid.NewGuid(),
                channelId,
                newsId,
                personId,
                createdOn = DateTime.UtcNow
            };

            await using var connection = new SqlConnection(this.connectionString);
            await connection.ExecuteAsync(sql, sqlData);
        }
    }
}