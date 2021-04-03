using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Core.GetAllReaders
{
    public interface IReadersRepository
    {
        Task<List<ReadersRepository.ReaderDto>> GetAllIdsAsync();
    }

    public class ReadersRepository : IReadersRepository
    {
        private string readerConnection;

        public ReadersRepository(IOptions<ConnectionOptions> options)
        {
            this.readerConnection = options.Value.SqlReader;
        }

        public async Task<List<ReaderDto>> GetAllIdsAsync()
        {
            var query = "SELECT * FROM persons";
            
            using (var connection = new SqlConnection(this.readerConnection))
            {
                var readers = await connection.QueryAsync<ReaderDto>(query);
                return readers.ToList();
            }
        }

        public class ReaderDto
        {
            public Guid Id { get; set; }
            public DateTime CreatedOn { get; set; }
        }
    }
}