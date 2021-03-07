using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Core.UpdateShowUpdatedNews
{
    class SettingsRepository : ISettingsRepository
    {
        private readonly string connectionString;

        public SettingsRepository(IOptions<ConnectionOptions> options)
        {
            this.connectionString = options.Value.SqlWriter;
        }

        public async Task UpdateShowUpdatedNews(Guid userId, UpdatedNews showUpdatedNews)
        {
            var sqlQuery = "UPDATE ReadersSettings SET ShowUpdatedNews = @showUpdatedNews WHERE PersonId = @userId";

            var sqlData = new
            {
                showUpdatedNews = showUpdatedNews.ToString(),
                userId
            };

            await using var connection = new SqlConnection(this.connectionString);
            await connection.ExecuteAsync(sqlQuery, sqlData);
        }

        public async Task UpdateSquashNewsUpdates(Guid userId, SquashUpdate squashNewsUpdates)
        {
            var sqlQuery = "UPDATE ReadersSettings SET SquashNewsUpdates = @squashNewsUpdates WHERE PersonId = @userId";

            var sqlData = new
            {
                squashNewsUpdates = squashNewsUpdates.ToString(),
                userId
            };

            await using var connection = new SqlConnection(this.connectionString);
            await connection.ExecuteAsync(sqlQuery, sqlData);
        }
    }
}