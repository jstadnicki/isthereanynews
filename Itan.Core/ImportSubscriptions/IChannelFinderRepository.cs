using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;

namespace Itan.Core.ImportSubscriptions
{
    public interface IChannelFinderRepository
    {
        Task<Guid> FindChannelIdByUrlAsync(string channelUrl);
    }

    class ChannelFinderRepository : IChannelFinderRepository
    {
        private readonly string connectionString;

        public ChannelFinderRepository(ConnectionOptions options)
        {
            this.connectionString = options.SqlReader;
        }

        public async Task<Guid> FindChannelIdByUrlAsync(string channelUrl)
        {
            var sql = "SELECT TOP 1 Id FROM channels WHERE url = @url";

            var sqlData = new
            {
                url = channelUrl
            };

            using var connection = new SqlConnection(this.connectionString);
            var executionResult = await connection.QueryAsync<Guid>(sql, sqlData);
            return executionResult.SingleOrDefault();
        }
    }
}