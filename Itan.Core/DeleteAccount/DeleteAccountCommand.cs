using System;
using System.Threading;
using System.Threading.Tasks;
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
    
    public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            return Unit.Value;
        }
    }
}