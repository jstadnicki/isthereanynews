using System;
using MediatR;

namespace Itan.Core.CreateNewUser
{
    public class CreateNewUserRequest : IRequest
    {
        public CreateNewUserRequest(string userId)
        {
            UserId = Guid.Parse(userId);
        }

        public Guid UserId { get; }
    }
}