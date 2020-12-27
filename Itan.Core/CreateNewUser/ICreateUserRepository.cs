using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Core.CreateNewUser
{
    public interface ICreateUserRepository
    {
        Task CreatePersonIfNotExists(Guid requestUserId);
    }

    class CreateUserRepository : ICreateUserRepository
    {
        private readonly string connectionString;

        public CreateUserRepository(IOptions<ConnectionOptions> options)
        {
            this.connectionString = options.Value.SqlWriter;
        }

        public async Task CreatePersonIfNotExists(Guid requestUserId)
        {
            var sql = $"if not exists (select * from Persons where id='{requestUserId.ToString()}')\n" +
                      "BEGIN\n" +
                      $"INSERT INTO Persons (Id, CreatedOn) VALUES ('{requestUserId.ToString()}','{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}')\n" +
                      "END";
            using var connection = new SqlConnection(this.connectionString);
            await connection.ExecuteAsync(sql);
        }
    }
}