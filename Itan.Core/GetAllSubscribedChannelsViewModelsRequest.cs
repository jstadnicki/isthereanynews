using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using MediatR;

namespace Itan.Core
{
    public class GetAllSubscribedChannelsViewModelsRequest : IRequest<List<ChannelViewModel>>
    {
        public string PersonId { get; set; }
    }

    public class GetAllSubscribedChannelsViewModelsRequestHandler : IRequestHandler<GetAllSubscribedChannelsViewModelsRequest, List<ChannelViewModel>>
    {
        private readonly string connectionString;

        public GetAllSubscribedChannelsViewModelsRequestHandler(ConnectionOptions options)
        {
            this.connectionString = options.SqlReader;
        }

        public async Task<List<ChannelViewModel>> Handle(
            GetAllSubscribedChannelsViewModelsRequest request,
            CancellationToken _)
        {
            var sqlQuery =
                "SELECT Channels.id, Channels.title, Channels.description, Channels.Url, COUNT(News.Id) as NewsCount\n" +
                "FROM ChannelsPersons\n" +
                "JOIN Channels ON ChannelsPersons.ChannelId = Channels.Id\n" +
                "LEFT JOIN News ON News.ChannelId = Channels.Id\n" +
                "WHERE ChannelsPersons.PersonId = @personId\n" +
                "GROUP BY Channels.Id, Channels.Title, Channels.Description, Channels.Url";
            var sqlData = new
            {
                personId = request.PersonId
            };

            using var connection = new SqlConnection(this.connectionString);
            var result = await connection.QueryAsync<ChannelViewModel>(sqlQuery, sqlData);
            return result.ToList();
        }
    }
}