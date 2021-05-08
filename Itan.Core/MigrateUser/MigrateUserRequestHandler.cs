using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using MediatR;
using Microsoft.Extensions.Options;

namespace Itan.Core.MigrateUser
{
    public class MigrateUserRequestHandler : IRequestHandler<MigrateUserRequest, Unit>
    {
        private readonly string writeConnection;

        public MigrateUserRequestHandler(IOptions<ConnectionOptions> options)
        {
            this.writeConnection = options.Value.SqlWriter;
        }

        public async Task<Unit> Handle(MigrateUserRequest request, CancellationToken cancellationToken)
        {
            await CreatePersonIfNotExistsAsync(request.UserId);
            await CreateSettingsIfNotExistsAsync(request.UserId);
            
            return Unit.Value;
        }

        private async Task CreateSettingsIfNotExistsAsync(Guid personId)
        {
            var query = "if not EXISTS(select * from readerssettings where personid = @personId)\n" +
                        "BEGIN\n" +
                        "insert into readerssettings (id, personid, ShowUpdatedNews, SquashNewsUpdates)\n" +
                        "values (@id, @personId, @showupdatednews, @squashNewsUpdates)\n"+
                        "END";
            var queryData = new
            {
                id = Guid.NewGuid(),
                personId = personId,
                showupdatednews = UpdatedNews.Show.ToString(),
                squashNewsUpdates = SquashUpdate.Show.ToString()
            };

            await using var connection = new SqlConnection(this.writeConnection);
            await connection.ExecuteAsync(query, queryData);
        }

        private async Task CreatePersonIfNotExistsAsync(Guid personId)
        {
            var query = "if not EXISTS(select * from persons where id = @id)\n" +
                        "BEGIN\n" +
                        "    INSERT INTO PERSONS (id, createdon) values (@id, @date)\n" +
                        "END";
            var queryData = new
            {
                id = personId,
                date = DateTime.UtcNow
            };

            await using var connection = new SqlConnection(this.writeConnection);
            await connection.ExecuteAsync(query, queryData);
        }
    }
}