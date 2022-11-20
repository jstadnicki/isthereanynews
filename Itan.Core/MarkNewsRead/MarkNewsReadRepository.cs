using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Core.MarkNewsRead
{
    internal class MarkNewsReadRepository : IMarkNewsReadRepository
    {
        private readonly string _connectionString;
 
        public MarkNewsReadRepository(IOptions<ConnectionOptions> options)
        {
            _connectionString = options.Value.SqlWriter;
        }

        public Task MarkReadAsync(Guid channelId, Guid newsId, Guid personId, IMarkNewsReadRepository.NewsReadType readType = IMarkNewsReadRepository.NewsReadType.Read )
            => MarkReadAsync(channelId, new Guid[] {newsId}, personId, readType);

        public async Task MarkReadAsync(Guid channelId, ICollection<Guid> newsIds, Guid personId, IMarkNewsReadRepository.NewsReadType readType)
        {
            var sql =
                "INSERT INTO ChannelNewsReads (Id, ChannelId, NewsId, PersonId, CreatedOn, ReadType)" +
                " VALUES (@id, @channelId, @newsId, @personId, @createdOn, @readType)";


            var sqlData = newsIds.Select(id => new
            {
                id = Guid.NewGuid(),
                channelId,
                newsId = id,
                personId,
                createdOn = DateTime.UtcNow,
                readType = readType.ToString()
            }).ToList();

            await using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(sql, sqlData);
        }
    }
}