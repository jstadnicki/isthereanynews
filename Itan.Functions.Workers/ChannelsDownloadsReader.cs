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
        private readonly string _connectionString;

        public ChannelsDownloadsReader(IOptions<ConnectionOptions> options)
        {
            Ensure.NotNull(options, nameof(options));
            _connectionString = options.Value.SqlReader;
        }

        public async Task<bool> Exists(Guid id, string sha256)
        {
            var checkForExistenceQuery = "SELECT * FROM ChannelDownloads WHERE ChannelId = @channelId AND SHA256 = @SHA256";
            var checkForExistenceQueryData = new {channelId = id, SHA256 = sha256};

            using var sqlConnection = new SqlConnection(_connectionString);
            var result = await sqlConnection.QuerySingleOrDefaultAsync(checkForExistenceQuery, checkForExistenceQueryData);
            return result != null;
        }
    }
}