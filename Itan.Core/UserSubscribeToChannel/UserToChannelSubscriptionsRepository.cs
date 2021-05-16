﻿using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Core.UserSubscribeToChannel
{
    class UserToChannelSubscriptionsRepository : IUserToChannelSubscriptionsRepository
    {
        private readonly string connectionString;

        public UserToChannelSubscriptionsRepository(IOptions<ConnectionOptions> options)
        {
            this.connectionString = options.Value.SqlWriter;
        }

        public async Task<int> CreateSubscriptionAsync(Guid requestChannelId, Guid requestUserId)
        {
            var sql =
                $"if not exists (select top 1 * from ChannelsPersons where ChannelId=@channelId and PersonId=@personId)\n" +
                "BEGIN\n" +
                "INSERT INTO ChannelsPersons (Id, ChannelId, PersonId, CreatedOn) \n" +
                "VALUES (@id,@channelId,@personId,@createdOn)" +
                "END";
            
            var sqlData = new
            {
                id = Guid.NewGuid(),
                channelId = requestChannelId,
                personId = requestUserId,
                createdOn = DateTime.UtcNow
            };
            
            using var connection = new SqlConnection(this.connectionString);
            return await connection.ExecuteAsync(sql, sqlData);
        }
    }
}