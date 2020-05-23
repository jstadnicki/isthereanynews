using System;
using System.Threading;
using System.Threading.Tasks;
using Itan.Core.Handlers;
using MediatR;

namespace Itan.Core.CreateNewUser
{
    public class CreateUserHandler : AsyncRequestHandler<CreateNewUserRequest>
    {
        private readonly ICreateUserRepository repository;

        public CreateUserHandler(ICreateUserRepository repository)
        {
            this.repository = repository;
        }


        protected override Task Handle(CreateNewUserRequest request, CancellationToken cancellationToken)
        {
            this.Validate(request);
            return this.repository.CreateNewPersonAsync(request.UserId);
        }

        private void Validate(CreateNewUserRequest request)
        {
            if (Guid.Empty == request.UserId)
            {
                throw new BadArgumentInRequestException(nameof(CreateUserHandler), nameof(request.UserId));
            }
        }
    }
}