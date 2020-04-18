using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Itan.Functions.Models;

public class ChannelProvider : IChannelsProvider
{
    private string connectionString;

    public ChannelProvider(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task<List<ChannelToDownload>> GetAllChannelsAsync()
    {
        using (var sqlConnection = new SqlConnection(this.connectionString))
        {
            var query = "SELECT c.Id, c.Url FROM Channels c";
            var queryResult = await sqlConnection.QueryAsync<ChannelToDownload>(query);
            var list = queryResult.ToList();
            return list;
        }        
    }
}