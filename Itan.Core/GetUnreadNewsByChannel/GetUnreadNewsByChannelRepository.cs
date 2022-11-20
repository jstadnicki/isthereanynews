using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Itan.Core.UpdateSquashNewsUpdates;
using Microsoft.Extensions.Options;

namespace Itan.Core.GetUnreadNewsByChannel
{
    class GetUnreadNewsByChannelRepository : IGetUnreadNewsByChannelRepository
    {
        private string _connection;

        public GetUnreadNewsByChannelRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connection = connectionOptions.Value.SqlReader;
        }

        public async Task<List<NewsHeader>> GetUnreadNewsAsync(
            string channelId,
            string personId,
            UpdatedNews updatedNews,
            SquashUpdate squashUpdate)
        {
            var hideUpdatesQuery = " and n.OriginalPostId is null";

            var empty = string.Empty;

            var query = " select n.Id, n.Title, n.Published, n.Published, n.Link, n.OriginalPostId" +
                        " from News n" +
                        " where n.Id not in(" +
                        " select cnr.NewsId" +
                        " from ChannelNewsReads cnr" +
                        " where cnr.ChannelId = @channelId" +
                        " and cnr.PersonId = @personId)" +
                        " and n.ChannelId = @channelId" +
                        (updatedNews == UpdatedNews.Ignore ? hideUpdatesQuery : empty) +
                        " order by n.Published desc";

            if (updatedNews == UpdatedNews.Show &&  squashUpdate == SquashUpdate.Squash)
            {
                query = " select t.Id, t.Title, t.Published, t.Link, t.OriginalPostId, rn" +
                        " into #ns" +
                        " from ( " +
                        " select n.Id, n.Title, n.Published, n.Link, n.OriginalPostId, ROW_NUMBER() over (PARTITION by link,OriginalPostId order by published desc) as rn" +
                        " from News n" +
                        " where n.Id not in" +
                        " ( select cnr.NewsId from ChannelNewsReads cnr" +
                        " where cnr.ChannelId = @channelId and cnr.PersonId = @personId)" +
                        " and n.ChannelId = @channelId" +
                        " )t " +
                        " where rn=1" +
                        " select l.* from #ns l" +
                        " left join #ns r on l.OriginalPostId = r.Id" +
                        " where r.Id is null";
            }

            var queryData = new
            {
                channelId,
                personId
            };

            using (var connection = new SqlConnection(_connection))
            {
                var news = await connection.QueryAsync<NewsHeader>(query, queryData);
                return news.ToList();
            }
        }
    }
}