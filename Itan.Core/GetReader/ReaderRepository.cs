using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Core.GetReader
{
    class ReaderRepository : IReaderRepository
    {
        private readonly string _readerConnection;

        public ReaderRepository(IOptions<ConnectionOptions> options)
        {
            _readerConnection = options.Value.SqlReader;
        }

        public async Task<List<ReaderSubscribedChannel>> GetSubscribedChannelsAsync(string personId)
        {
            var query =
                " select c.Id, c.Title, cp.CreatedOn, cnr.ReadType, count(*) as ActionsCount\n" +
                " into #TT\n" +
                "     from ChannelsPersons cp\n" +
                " join Channels c\n" +
                "     on cp.ChannelId = c.Id\n" +
                " join ChannelNewsReads cnr\n" +
                "     on cnr.ChannelId = c.Id AND cnr.PersonId = cp.PersonId\n" +
                " where cp.PersonId=@personId\n" +
                " group by c.Id,  c.Title, cp.CreatedOn, cnr.ReadType\n" +
                "\n" +
                " select * into #PT FROM\n" +
                " (\n" +
                "     select Id, Title, ReadType, ActionsCount, CreatedOn\n" +
                "         from #TT\n" +
                " ) src\n" +
                " pivot(\n" +
                "     avg(ActionsCount)\n" +
                " for ReadType in ([Click],[Read],[Skip])\n" +
                " ) dst\n" +
                " select pt.Id, pt.Title, pt.CreatedOn, pt.Click, pt.[Read], pt.Skip, Count(*) as TotalNewsCount\n"+ 
                " from #PT pt\n"+
                " join news n\n"+
                "     on n.ChannelId = pt.id\n"+
                " group by pt.Id, pt.Title, pt.CreatedOn, pt.Click, pt.[Read], pt.Skip";

            var queryData = new
            {
                personId
            };
            
            using (var connection = new SqlConnection(_readerConnection))
            {
                var queryResult = await connection.QueryAsync<ReaderSubscribedChannel>(query,queryData);
                return queryResult.ToList();
            }
        }
    }
}