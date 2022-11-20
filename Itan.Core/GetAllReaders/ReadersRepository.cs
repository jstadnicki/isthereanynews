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
    public class ReadersRepository : IReadersRepository
    {
        private string _readerConnection;

        public ReadersRepository(IOptions<ConnectionOptions> options)
        {
            _readerConnection = options.Value.SqlReader;
        }

        public async Task<List<ReaderDto>> GetAllIdsAsync()
        {
            var query = "SELECT * FROM persons";
            
            using (var connection = new SqlConnection(_readerConnection))
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