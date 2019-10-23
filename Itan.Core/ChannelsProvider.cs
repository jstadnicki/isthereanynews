using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Itan.Core
{
    public class ChannelsProvider
    {
        public List<ChannelViewModel> GetAll()
        {
            var connectionString = "server=.;database=itan;User Id=itanreaduser;password=12qw!@QW";
            using (var connection = new SqlConnection(connectionString))
            {
                var result = connection.Query<ChannelViewModel>("select c.id,c.title,c.description,c.Url,count(n.Id) as NewsCount from Channels c left join News n on n.ChannelId = c.Id group by c.Id,c.Title,c.Description, c.Url");
                return result.ToList();
            }
        }
    }
}
