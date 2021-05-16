using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using MediatR;
using Microsoft.Extensions.Options;

namespace Itan.Core.GetFollowerActivity
{
    public class GetFollowerActivityQueryHandler : IRequestHandler<GetFollowerActivityQuery, List<FollowerActivityViewModel>>
    {
        private readonly string readerConnection;

        public GetFollowerActivityQueryHandler(IOptions<ConnectionOptions> options)
        {
            this.readerConnection = options.Value.SqlReader;
        }

        public async Task<List<FollowerActivityViewModel>> Handle(GetFollowerActivityQuery request, CancellationToken cancellationToken)
        {
            var date = DateTime.UtcNow.AddDays(-7);
            
            var query =
                "SELECT \n" +
                "cnr.Id, cnr.newsid, cnr.createdon, cnr.ReadType, n.title, n.published, n.link, n.ChannelId\n" +
                "FROM [dbo].[ChannelNewsReads] cnr\n" +
                "    join news n\n" +
                "on cnr.newsid = n.id\n" +
                "\n" +
                "where cnr.CreatedOn> @date\n" +
                "and PersonId=@personId\n";

            var queryData = new
            {
                date = DateTime.UtcNow.AddDays(-7),
                personId = request.PersonId
            };

            using (var connection = new SqlConnection(this.readerConnection))
            {
                var readerAsync = await connection.QueryAsync<FollowerActivityViewModel>(query, queryData);
                var list = readerAsync.ToList();
                return list;
            }
        }
    }
}