using System;
using System.Threading;
using System.Threading.Tasks;
using Itan.Core.Handlers;
using MediatR;

namespace Itan.Core.CreateNewUser
{
    public class CreateUserHandler : IRequestHandler<CreateNewUserRequest>
    {
        private readonly ICreateUserRepository repository;
        
        public CreateUserHandler(ICreateUserRepository repository)
        {
            this.repository = repository;
        }
        private void Validate(CreateNewUserRequest request)
        {
            if (Guid.Empty == request.UserId)
            {
                throw new BadArgumentInRequestException(nameof(CreateUserHandler), nameof(request.UserId));
            }
        }

        public async Task<Unit> Handle(CreateNewUserRequest request, CancellationToken cancellationToken)
        {
            this.Validate(request);
            await this.repository.CreatePersonIfNotExists(request.UserId);
            return await Task.FromResult(new Unit());
        }
    }
}