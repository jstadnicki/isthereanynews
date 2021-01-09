﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Core.GetUnreadNewsByChannel
{
    class GetUnreadNewsByChannelRepository : IGetUnreadNewsByChannelRepository
    {
        private string connection;

        public GetUnreadNewsByChannelRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            this.connection = connectionOptions.Value.SqlReader;
        }

        public async Task<List<NewsHeader>> GetUnreadNewsAsync(string channelId, string personId)
        {
            var query = " select n.Id, n.Title, n.Published, n.Published, n.Link, n.OriginalPostId"+
                        " from News n" +
                        " where n.Id not in(" +
                        " select cnr.NewsId" +
                        " from ChannelNewsReads cnr" +
                        " where cnr.ChannelId = @channelId" +
                        " and cnr.PersonId = @personId)"+
                        " and n.ChannelId = @channelId" +
                        " order by n.Published desc";

            var queryData = new
            {
                channelId,
                personId
            };

            using (var connection = new SqlConnection(this.connection))
            {
                var news = await connection.QueryAsync<NewsHeader>(query, queryData);
                return news.ToList();
            }
        }
    }
}