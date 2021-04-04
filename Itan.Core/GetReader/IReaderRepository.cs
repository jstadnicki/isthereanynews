using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Microsoft.Extensions.Options;

namespace Itan.Core.GetReader
{
    public interface IReaderRepository
    {
        Task<List<string>> GetSubscribedChannelsAsync(string personId);
    }

    class ReaderRepository : IReaderRepository
    {
        private readonly string readerConnection;

        public ReaderRepository(IOptions<ConnectionOptions> options)
        {
            this.readerConnection = options.Value.SqlReader;
        }

        public async Task<List<string>> GetSubscribedChannelsAsync(string personId)
        {
            var query = " SELECT c.Title " +
                        " FROM ChannelsPersons cp" +
                        " JOIN Channels c " +
                        " ON cp.ChannelId = c.Id " +
                        " WHERE cp.PersonId=@personId";

            var queryData = new
            {
                personId
            };
            
            using (var connection = new SqlConnection(this.readerConnection))
            {
                var queryResult = await connection.QueryAsync<string>(query,queryData);
                return queryResult.ToList();
            }
        }
    }
}