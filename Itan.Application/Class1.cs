using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Itan.Application
{
    public class ChannelsProvider
    {
        public List<ChannelViewModel> GetAll()
        {
            var connectionString = "server=.\\sql2017;database=itan;User Id=itanreaduser;password=12qw!@QW";
            using (var connection = new SqlConnection(connectionString))
            {
                var result = connection.Query<ChannelViewModel>("SELECT * FROM CHANNELS");
                return result.ToList();
            }
        }
    }

    public class ChannelViewModel
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public Guid Id { get; set; }
        public int NewsCount { get; set; }
    }
}
