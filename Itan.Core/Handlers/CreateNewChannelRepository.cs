using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;

namespace Itan.Core.Handlers
{
    public class CreateNewChannelRepository : ICreateNewChannelRepository
    {
        private readonly string connectionString;

        public CreateNewChannelRepository(ConnectionOptions options)
        {
            this.connectionString = options.SqlWriter;
        }
        public async Task<Guid> SaveAsync(string url, Guid submitterId)
        {
            var sql = "if not exists " +
                      "(select top 1 url from channels where url=@url) " +
                      "begin " +
                      "INSERT INTO Channels (Id, Url, CreatedOn, ModifiedOn) VALUES (@channelId, @url, @date, @date)" +
                      "INSERT INTO ChannelsSubmitters (Id, ChannelId, PersonId, CreatedOn) VALUES (@channelSubmitterId, @channelId, @personId, @date)" +
                      "end";

            var sqlData = new
            {
                channelId = Guid.NewGuid(),
                channelSubmitterId = Guid.NewGuid(),
                url = url,
                personId = submitterId,
                date = DateTime.UtcNow,
            };

            await using var connection = new SqlConnection(this.connectionString);
            await connection.ExecuteAsync(sql, sqlData);
            return sqlData.channelId;
        }
    }
}