using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Core.GetUnreadNewsByChannel
{
    class ReaderSettingsRepository : IReaderSettingsRepository
    {
        private string connectionString;

        public ReaderSettingsRepository(IOptions<ConnectionOptions> options)
        {
            this.connectionString = options.Value.SqlReader;
        }

        public async Task<ReaderSettings> GetAsync(string personId)
        {
            var query = "SELECT * FROM ReadersSettings WHERE PersonId = @personId";
            var queryData = new
            {
                personId
            };

            using var connection = new SqlConnection(this.connectionString);
            var queryResult = await connection.QueryAsync<ReaderSettings>(query, queryData);
            var readerSettings = queryResult.Single();
            return readerSettings;
        }

        public async Task CreateDefaultValuesAsync(Guid requestUserId)
        {
            const string query = "INSERT INTO ReadersSettings (Id, PersonId, ShowUpdatedNews, SquashNewsUpdates) VALUES (@id, @personId, @show, @squash)";
            var queryData = new
            {
                id = Guid.NewGuid().ToString(),
                personId = requestUserId,
                show = UpdatedNews.Show.ToString(),
                squash = SquashUpdate.Show.ToString()
            };

            await using var connection = new SqlConnection(this.connectionString);
            await connection.ExecuteAsync(query, queryData);
        }
    }
}