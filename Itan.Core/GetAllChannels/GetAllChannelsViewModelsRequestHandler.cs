using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using MediatR;
using Microsoft.Extensions.Options;

namespace Itan.Core.GetAllChannels
{
    public class GetAllChannelsViewModelsRequestHandler : IRequestHandler<GetAllChannelsViewModelsRequest, List<ChannelViewModel>>
    {
        private string connectionString;

        public GetAllChannelsViewModelsRequestHandler(IOptions<ConnectionOptions> options)
        {
            this.connectionString = options.Value.SqlReader;
        }

        public async Task<List<ChannelViewModel>> Handle(
            GetAllChannelsViewModelsRequest _,
            CancellationToken __)
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                var sqlQuery = 
                    "SELECT c.id, c.title, c.description, c.Url, count(n.Id) as NewsCount"+
                    " FROM Channels c" +
                    " LEFT JOIN News n ON" +
                    " (n.ChannelId = c.Id AND n.OriginalPostId IS NULL )" +
                    " GROUP BY c.Id,c.Title,c.Description, c.Url";
                var result = await connection.QueryAsync<ChannelViewModel>(sqlQuery);
                return result.ToList();
            }
        }
    }
}