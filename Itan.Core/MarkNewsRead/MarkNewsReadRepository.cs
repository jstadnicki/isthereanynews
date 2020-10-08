using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;

namespace Itan.Core.MarkNewsRead
{
    internal class MarkNewsReadRepository : IMarkNewsReadRepository
    {
        private string connectionString;
 
        public MarkNewsReadRepository(ConnectionOptions options)
        {
            this.connectionString = options.SqlWriter;
        }
        public async Task MarkReadAsync(Guid channelId, Guid newsId, Guid personId)
        {
            var sql =
                $"if not exists (select top 1 * from ChannelNewsReads where ChannelId=@channelId AND NewsId=@newsId AND PersonId=@personId)\n" +
                "BEGIN\n" +
                "INSERT INTO ChannelNewsReads (Id, ChannelId, NewsId, PersonId, CreatedOn) \n" +
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