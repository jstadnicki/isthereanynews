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
                foreach (var outline in subs.A.Outline)
                {
                    var query =
                        "INSERT INTO CHANNELS (Id,Url,CreatedOn,ModifiedOn) VALUES (@id,@url,@created,@modified)";
                    var data = new
                    {
                        id = Guid.NewGuid(),
                        url = outline.Htmlurl,
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
        public string T { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Htmlurl { get; set; }
        public string XmlUrl { get; set; }
    }

    public class A
    {
        public List<Outline> Outline { get; set; }
    }

    public class RootObject
    {
        public A A { get; set; }
    }
}
