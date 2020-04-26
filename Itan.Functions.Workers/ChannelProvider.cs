using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Itan.Functions.Models;
using Microsoft.Extensions.Options;

namespace Itan.Functions.Workers
{
    public class ChannelProvider : IChannelsProvider
    {
        private readonly IConnectionOptions connectionOptions;

        public ChannelProvider(IOptions<ConnectionOptions> connectionOptions)
        {
            Ensure.NotNull(connectionOptions, nameof(connectionOptions));

            this.connectionOptions = connectionOptions.Value;
        }

        public async Task<List<ChannelToDownload>> GetAllChannelsAsync()
        {
            using (var sqlConnection = new SqlConnection(this.connectionOptions.SqlReader))
            {
                var query = "SELECT c.Id, c.Url FROM Channels c";
                var queryResult = await sqlConnection.QueryAsync<ChannelToDownload>(query);
                var list = queryResult.ToList();
                return list;
            }        
        }
    }
}