using System;
using MediatR;

namespace Itan.Core.MigrateUser
{
    public class MigrateUserRequest : IRequest
    {
        public Guid UserId { get; }

        public MigrateUserRequest(Guid userId)
        {
            UserId = userId;
        }
    }
}