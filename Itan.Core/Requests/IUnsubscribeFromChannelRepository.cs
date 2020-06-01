using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using MediatR;

namespace Itan.Api.Controllers
{
    public interface IUnsubscribeFromChannelRepository
    {
        Task DeleteSubscriptionAsync(Guid channelId, Guid personId);
    }

    internal class UnsubscribeFromChannelRepository : IUnsubscribeFromChannelRepository
    {
        private readonly string connectionString;

        public UnsubscribeFromChannelRepository(ConnectionOptions options)
        {
            this.connectionString = options.SqlWriter;
        }

        public async Task DeleteSubscriptionAsync(Guid channelId, Guid personId)
        {
            var sql = "DELETE FROM ChannelsPersons where ChannelId = @channelId and PersonId = @personId";
            var sqlData = new
            {
                channelId = channelId,
                personId = personId
            };

            using var connection = new SqlConnection(this.connectionString);
            await connection.ExecuteAsync(sql, sqlData);
        }
    }
}