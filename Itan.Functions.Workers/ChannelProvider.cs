using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Functions.Workers
{
    public class ChannelProvider : IChannelsProvider
    {
        private readonly string _connectionString;

        public ChannelProvider(IOptions<ConnectionOptions> connectionOptions)
        {
            Ensure.NotNull(connectionOptions, nameof(connectionOptions));

            _connectionString = connectionOptions.Value.SqlReader;
        }

        public async Task<List<ChannelToDownload>> GetAllChannelsAsync()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                var query = "SELECT c.Id, c.Url FROM Channels c";
                var queryResult = await sqlConnection.QueryAsync<ChannelToDownload>(query);
                var list = queryResult.ToList();
                return list;
            }        
        }
    }
}