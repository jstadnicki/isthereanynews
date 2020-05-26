using System;
using System.Threading.Tasks;

namespace Itan.Core.CreateNewUser
{
    public interface ICreateUserRepository
    {
        Task CreateNewPersonAsync(Guid requestUserId);
    }

    class CreateUserRepository : ICreateUserRepository
    {
        public Task CreateNewPersonAsync(Guid requestUserId)
        {
            return Task.CompletedTask;
        }
    }
}