using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Itan.Functions.Workers.Model;
using Itan.Functions.Workers.Wrappers;
using Microsoft.Extensions.Options;

namespace Itan.Functions.Workers
{
    public class ChannelsDownloadsWriter : IChannelsDownloadsWriter
    {
        private string sqlConnectionStringWriter;
        private ILoger<ChannelsDownloadsWriter> log;

        public ChannelsDownloadsWriter(IOptions<ConnectionOptions> options, ILoger<ChannelsDownloadsWriter> log)
        {
            Ensure.NotNull(options, nameof(options));
            Ensure.NotNull(log, nameof(log));
        
            this.sqlConnectionStringWriter = options.Value.SqlWriter;
            this.log = log;
        }

        public async Task InsertAsync(DownloadDto data)
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
}