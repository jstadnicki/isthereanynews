using CodeHollow.FeedReader;

using Newtonsoft.Json;

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Itan.Ui.Services
{
    public class NewsContentViewModel
    {
        public bool Downloaded { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool Visible { get; set; }

        public string CssVisibilityClass => Downloaded && Visible ? string.Empty : "d-none";

        public async Task OnClickAsync()
        {
            if (!Downloaded)
            {
                try
                {
                    var client = new HttpClient();
                    var j = await client.GetStringAsync(Url);
                    var jsonResolver = new IgnorableSerializerContractResolver();
                    jsonResolver.Ignore(typeof(FeedItem), "SpecificItem");
                    var si = JsonConvert.DeserializeObject<FeedItem>(j, new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ContractResolver = jsonResolver
                    });
                    Content = si.Description.ToString();
                    Downloaded = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            Visible = !Visible;
        }
    }
}
