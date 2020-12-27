using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using MediatR;
using Microsoft.Extensions.Options;

namespace Itan.Core
{
    public class GetAllSubscribedChannelsViewModelsRequest : IRequest<List<ChannelViewModel>>
    {
        public string PersonId { get; set; }
    }

    public class GetAllSubscribedChannelsViewModelsRequestHandler : IRequestHandler<GetAllSubscribedChannelsViewModelsRequest, List<ChannelViewModel>>
    {
        private readonly string connectionString;

        public GetAllSubscribedChannelsViewModelsRequestHandler(IOptions<ConnectionOptions> options)
        {
            this.connectionString = options.Value.SqlReader;
        }

        public async Task<List<ChannelViewModel>> Handle(
            GetAllSubscribedChannelsViewModelsRequest request,
            CancellationToken _)
        {
            var sqlQuery =
                " select c.Id, c.Title, c.Description, c.Url, count(n.Id) as NewsCount from ChannelsPersons cp" +
                " join News n " +
                " on n.ChannelId = cp.ChannelId " +
                " join Channels c " +
                " on c.Id = cp.ChannelId " +
                $" WHERE cp.PersonId = @personId " +
                " and " +
                " n.Id not in ( " +
                " select cnr.NewsId from ChannelNewsReads cnr " +
                $" where cnr.PersonId = @personId" +
                " ) " +
                " GROUP BY c.Id, c.Title, c.Description, c.Url ";

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