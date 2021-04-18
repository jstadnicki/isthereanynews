using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using MediatR;
using Microsoft.Extensions.Options;

namespace Itan.Api.Controllers
{
    public class UnfollowPersonCommandHandler:IRequestHandler<UnfollowPersonCommand, Unit>
    {
        private readonly string writeConnection;

        public UnfollowPersonCommandHandler(IOptions<ConnectionOptions> options)
        {
            this.writeConnection = options.Value.SqlWriter;
        }

        public async Task<Unit> Handle(UnfollowPersonCommand request, CancellationToken cancellationToken)
        {
            var query = "DELETE FROM PersonsPersons WHERE TargetPersonId=@targetPersonId and FollowerPersonId=@followerPersonId";
            var queryData = new
            {
                targetPersonId=request.TargetPersonId,
                followerPersonId=request.ActualPersonId
            };

            await using var connection = new SqlConnection(this.writeConnection);
            await connection.ExecuteAsync(query, queryData);

            return Unit.Value;
        }
    }
}