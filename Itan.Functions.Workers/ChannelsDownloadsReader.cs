using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Functions.Workers
{
    public class ChannelsDownloadsReader : IChannelsDownloadsReader
    {
        private readonly string connectionString;

        public ChannelsDownloadsReader(IOptions<ConnectionOptions> options)
        {
            Ensure.NotNull(options, nameof(options));
            this.connectionString = options.Value.SqlReader;
        }

        public async Task<bool> Exists(Guid id, string SHA256)
        {
            var checkForExistenceQuery = "SELECT * FROM ChannelDownloads WHERE ChannelId = @channelId AND SHA256 = @SHA256";
            var checkForExistenceQueryData = new {channelId = id, SHA256 = SHA256};

            using var sqlConnection = new SqlConnection(this.connectionString);
            var result = await sqlConnection.QuerySingleOrDefaultAsync(checkForExistenceQuery, checkForExistenceQueryData);
            return result != null;
        }
    }
}