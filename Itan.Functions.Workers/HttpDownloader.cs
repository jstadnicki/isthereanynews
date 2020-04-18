using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Itan.Functions.Workers
{
    public class HttpDownloader : IHttpDownloader
    {
        private readonly ILoger log;

        public HttpDownloader(ILoger log)
        {
            this.log = log;
        }

        public async Task<string> GetStringAsync(string url)
        {
            var client = HttpClientFactory.Create();
            try
            {
                return await client.GetStringAsync(url);
            }
            catch (Exception e)
            {
                this.log.LogCritical(e.ToString());
            }

            return string.Empty;
        }
    }
}