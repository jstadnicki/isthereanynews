﻿using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Core.ImportSubscriptions
{
    class ChannelFinderRepository : IChannelFinderRepository
    {
        private readonly string _connectionString;

        public ChannelFinderRepository(IOptions<ConnectionOptions> options)
        {
            _connectionString = options.Value.SqlReader;
        }

        public async Task<Guid> FindChannelIdByUrlAsync(string channelUrl, IChannelFinderRepository.Match match = IChannelFinderRepository.Match.Exact )
        {
            var sql = $"SELECT TOP 1 Id FROM channels WHERE url {(match==IChannelFinderRepository.Match.Exact?GetExactUrlParam():GetLikeUrlParam())}";

            var sqlData = new
            {
                url = match==IChannelFinderRepository.Match.Exact?GetValueExact(channelUrl):GetValueLike(channelUrl)
            };

            using var connection = new SqlConnection(_connectionString);
            var executionResult = await connection.QueryAsync<Guid>(sql, sqlData);
            return executionResult.SingleOrDefault();
        }

        private string GetValueLike(string channelUrl) => $"%{channelUrl}%";
        private string GetValueExact(string channelUrl) => channelUrl;

        private string GetLikeUrlParam() => "LIKE @url";
        private string GetExactUrlParam() => " = @url";
    }
}