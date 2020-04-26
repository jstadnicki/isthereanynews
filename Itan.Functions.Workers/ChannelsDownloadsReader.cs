using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;

namespace Itan.Functions.Workers
{
    public class ChannelsDownloadsReader : IChannelsDownloadsReader
    {
        private string sqlConnectionStringReader;

        public ChannelsDownloadsReader(IOptions<ConnectionOptions> options)
        {
            Ensure.NotNull(options, nameof(options));
            this.sqlConnectionStringReader = options.Value.SqlReader;
        }

        public async Task<bool> Exists(Guid id, int hashCode)
        {
            var checkForExistenceQuery = "SELECT * FROM ChannelDownloads WHERE ChannelId = @channelId AND HashCode = @hashCode";
            var checkForExistenceQueryData = new {channelId = id, hashCode = hashCode};

            using var sqlConnection = new SqlConnection(this.sqlConnectionStringReader);
            var result = await sqlConnection.QuerySingleOrDefaultAsync(checkForExistenceQuery, checkForExistenceQueryData);
            return result != null;
        }
    }
}