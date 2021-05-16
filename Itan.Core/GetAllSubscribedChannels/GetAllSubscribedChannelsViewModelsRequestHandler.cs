using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using MediatR;
using Microsoft.Extensions.Options;

namespace Itan.Core.GetAllSubscribedChannels
{
    public class
        GetAllSubscribedChannelsViewModelsRequestHandler : IRequestHandler<GetAllSubscribedChannelsViewModelsRequest,
            List<SubscribedChannelViewModel>>
    {
        private readonly string connectionString;
        private readonly IReaderSettingsRepository readerSettingsRepository;

        public GetAllSubscribedChannelsViewModelsRequestHandler(
            IOptions<ConnectionOptions> options,
            IReaderSettingsRepository readerSettingsRepository)
        {
            this.readerSettingsRepository = readerSettingsRepository;
            this.connectionString = options.Value.SqlReader;
        }

        public async Task<List<SubscribedChannelViewModel>> Handle(
            GetAllSubscribedChannelsViewModelsRequest request,
            CancellationToken _)
        {
            var readerSettings = await this.readerSettingsRepository.GetAsync(request.PersonId);
            var queryNews = string.Empty;

            if (readerSettings.ShowUpdatedNews == UpdatedNews.Ignore)
            {
                queryNews = " AND n.OriginalPostId is null";
            }


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
                " )" +
                queryNews +
                " GROUP BY c.Id, c.Title, c.Description, c.Url ";

            if (readerSettings.ShowUpdatedNews == UpdatedNews.Show && readerSettings.SquashNewsUpdates == SquashUpdate.Squash)
            {
                sqlQuery = " SELECT ChannelId as Id, Title, Description, Url, Count(*) as NewsCount FROM\n" +
                           " (\n" +
                           " select n.ChannelId, n.Id, n.OriginalPostId, ROW_NUMBER() over (PARTITION by link,OriginalPostId order by published desc) as RowN, c.Title, c.Description, c.Url\n" +
                           " from News n\n" +
                           " join ChannelsPersons cp\n" +
                           " on cp.ChannelId = n.ChannelId and cp.PersonId = @personId\n" +
                           " join Channels c\n" +
                           " on cp.ChannelId = c.Id\n" +
                           " where n.Id not in (\n" +
                           " select cnr.NewsId\n" +
                           " from ChannelNewsReads cnr\n" +
                           " where cnr.PersonId = @personId\n" +
                           " )\n" +
                           " )TT\n" +
                           " WHERE TT.RowN=1\n" +
                           " GROUP BY ChannelId, Title, Description, Url";
            }

            var sqlData = new
            {
                personId = request.PersonId
            };

            using var connection = new SqlConnection(this.connectionString);
            var result = await connection.QueryAsync<SubscribedChannelViewModel>(sqlQuery, sqlData);
            return result.ToList();
        }
    }
}