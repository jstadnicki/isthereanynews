﻿using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using MediatR;
using Microsoft.Extensions.Options;

namespace Itan.Core.FollowPerson
{
    public class FollowPersonCommandHandler : IRequestHandler<FollowPersonCommand, Unit>
    {
        private readonly string _writerConnection;

        public FollowPersonCommandHandler(IOptions<ConnectionOptions> options)
        {
            _writerConnection = options.Value.SqlWriter;
        }
        public async Task<Unit> Handle(FollowPersonCommand request, CancellationToken cancellationToken)
        {
            var query =
                "if not exists (select * from PersonsPersons where TargetPersonId=@TargetPersonId AND FollowerPersonId=@FollowerPersonId)\n" +
                "BEGIN\n" +
                "INSERT INTO PersonsPersons (Id, TargetPersonId, FollowerPersonId, CreatedOn)\n" +
                "VALUES (@Id, @TargetPersonId, @FollowerPersonId, @CreatedOn)\n" +
                "END";

            var queryData = new
            {
                Id = Guid.NewGuid(),
                TargetPersonId = request.TargetPersonId,
                FollowerPersonId = request.ActualPersonId,
                CreatedOn = DateTime.UtcNow
            };

            await using var connection = new SqlConnection(_writerConnection);
            await connection.ExecuteAsync(query, queryData);

            return Unit.Value;
        }
    }
}