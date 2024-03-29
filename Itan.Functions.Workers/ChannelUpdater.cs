﻿using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Itan.Wrappers;
using Microsoft.Extensions.Options;

namespace Itan.Functions.Workers
{
    public class ChannelUpdater : IChannelUpdater
    {
        private readonly string _connectionString;
        private readonly ILoger<ChannelUpdater> _loger;

        public ChannelUpdater(IOptions<ConnectionOptions> options, ILoger<ChannelUpdater> loger)
        {
            Ensure.NotNull(options, nameof(options));
            Ensure.NotNull(loger, nameof(loger));

            _loger = loger;
            _connectionString = options.Value.SqlWriter;
        }

        public async Task Update(ChannelUpdate message)
        {
            var query = "UPDATE Channels SET Title = @title, Description = @description, ModifiedOn = @modified WHERE Id = @id";

            var queryData = new
            {
                title = message.Title,
                description = message.Description,
                modified = DateTime.UtcNow,
                id = message.Id
            };

            try
            {
                await using var sqlConnection = new SqlConnection(_connectionString);
                await sqlConnection.ExecuteAsync(query, queryData);
            }
            catch (Exception e)
            {
                _loger.LogCritical(e.ToString());
            }
        }
    }
}