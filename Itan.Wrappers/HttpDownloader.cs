using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Itan.Common;

namespace Itan.Wrappers
{
    public class HttpDownloader : IHttpDownloader
    {
        private readonly ILoger<HttpDownloader> _log;

        public HttpDownloader(ILoger<HttpDownloader> log)
        {
            Ensure.NotNull(log, nameof(log));
            _log = log;
        }

        public async Task<string> GetStringAsync(string url)
        {
            var client = HttpClientFactory.Create();
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("www.isthereanynews.com")));

            try
            {
                return await client.GetStringAsync(url);
            }
            catch (Exception e)
            {
                _log.LogCritical(e.Message);
                return string.Empty;
                //throw new ItanFailedToDownloadChannel($"Failed to download feed from channel: {url}", e);
            }
        }
    }
}