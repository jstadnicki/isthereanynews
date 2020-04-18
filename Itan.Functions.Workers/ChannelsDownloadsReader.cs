using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

public class ChannelsDownloadsReader : IChannelsDownloadsReader
{
    private string sqlConnectionStringReader;

    public ChannelsDownloadsReader(string sqlConnectionStringReader)
    {
        this.sqlConnectionStringReader = sqlConnectionStringReader;
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