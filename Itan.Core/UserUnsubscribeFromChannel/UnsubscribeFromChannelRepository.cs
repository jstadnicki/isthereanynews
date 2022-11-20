using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Core.UserUnsubscribeFromChannel
{
    internal class UnsubscribeFromChannelRepository : IUnsubscribeFromChannelRepository
    {
        private readonly string _connectionString;

        public UnsubscribeFromChannelRepository(IOptions<ConnectionOptions> options)
        {
            _connectionString = options.Value.SqlWriter;
        }

        public async Task DeleteSubscriptionAsync(Guid channelId, Guid personId)
        {
            var sql = "DELETE FROM ChannelsPersons where ChannelId = @channelId and PersonId = @personId";
            var sqlData = new
            {
                channelId = channelId,
                personId = personId
            };

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, sqlData);
        }
    }
}