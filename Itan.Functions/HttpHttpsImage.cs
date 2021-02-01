using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Itan.Functions
{
    public static class HttpHttpsImage
    {
        [FunctionName("HttpHttpsImage")]
        public static Task<HttpResponseMessage> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")]
            HttpRequest request,
            ILogger log)
        {
            var url = request.Query["url"];
            if (string.IsNullOrWhiteSpace(url))
            {
                return new Task<HttpResponseMessage>(
                    _ => new HttpResponseMessage {StatusCode = HttpStatusCode.BadRequest}, null);
            }
            
            return HttpClientFactory.Create().GetAsync(url);
        }
    }
}