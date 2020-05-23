using System;
using System.Threading.Tasks;

namespace Itan.Core.CreateNewUser
{
    public interface ICreateUserRepository
    {
        Task CreateNewPersonAsync(Guid requestUserId);
    }
}