using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using Dapper;

namespace ImportOpml
{
    class Program
    {
        static void Main(string[] args)
        {
            var substrictions = File.ReadAllText("subscriptions.json");
            var subs = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(substrictions);
            Console.WriteLine(subs);

            using (var sqlconnection = new SqlConnection("Server=.;Database=itan;user=ITAN;password=1234qwer!@#$QWER;"))
            {
                foreach (var outline in subs.a.outline)
                {
                    var query =
                        "INSERT INTO CHANNELS (Id,Url,CreatedOn,ModifiedOn) VALUES (@id,@url,@created,@modified)";
                    var data = new
                    {
                        id = Guid.NewGuid(),
                        url = outline.htmlurl,
                        created = DateTime.UtcNow,
                        modified = DateTime.UtcNow
                    };
                    sqlconnection.Execute(query, data);
                }
            }

        }
    }

    public class Outline
    {
        public string t { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string htmlurl { get; set; }
        public string xmlUrl { get; set; }
    }

    public class A
    {
        public List<Outline> outline { get; set; }
    }

    public class RootObject
    {
        public A a { get; set; }
    }
}
