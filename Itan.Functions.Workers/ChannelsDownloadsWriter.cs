using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Itan.Functions.Workers;

public class ChannelsDownloadsWriter : IChannelsDownloadsWriter
{
    private string sqlConnectionStringWriter;
    private ILoger log;

    public ChannelsDownloadsWriter(string sqlConnectionStringWriter, ILoger log)
    {
        this.sqlConnectionStringWriter = sqlConnectionStringWriter;
        this.log = log;
    }

    public async Task InsertAsync(object data)
    {
        var query = "INSERT INTO ChannelDownloads (Id, ChannelId, Path, CreatedOn, HashCode) VALUES (@id, @channelId, @path, @createdOn, @hashCode)";

        try
        {
            using (var sqlConnection = new SqlConnection(query))
            {
                await sqlConnection.ExecuteAsync(this.sqlConnectionStringWriter, data);
            }
        }
        catch (Exception e)
        {
            this.log.LogCritical(e.ToString());
            throw;
        }
    }
}