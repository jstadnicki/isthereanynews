﻿using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Itan.Functions.Workers.Model;
using Itan.Wrappers;
using Microsoft.Extensions.Options;

namespace Itan.Functions.Workers
{
    public class ChannelsDownloadsWriter : IChannelsDownloadsWriter
    {
        private readonly string _connectionString;
        private ILoger<ChannelsDownloadsWriter> _log;

        public ChannelsDownloadsWriter(IOptions<ConnectionOptions> options, ILoger<ChannelsDownloadsWriter> log)
        {
            Ensure.NotNull(options, nameof(options));
            Ensure.NotNull(log, nameof(log));
        
            _connectionString = options.Value.SqlWriter;
            _log = log;
        }

        public async Task InsertAsync(DownloadDto data)
        {
            var query = "INSERT INTO ChannelDownloads (Id, ChannelId, Path, CreatedOn, SHA256) VALUES (@id, @channelId, @path, @createdOn, @SHA)";

            try
            {
                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    await sqlConnection.ExecuteAsync(query, data);
                }
            }
            catch (Exception e)
            {
                _log.LogCritical(e.ToString());
                throw;
            }
        }
    }
}