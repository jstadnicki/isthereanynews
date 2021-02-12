using System;
using MediatR;

namespace Itan.Core.DeleteAccount
{
    public class DeleteAccountCommand : IRequest
    {
        public Guid UserId { get; }

        public DeleteAccountCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}