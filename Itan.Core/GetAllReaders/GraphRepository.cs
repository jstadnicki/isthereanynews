using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Itan.Core.DeleteAccount;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace Itan.Core.GetAllReaders
{
    class GraphRepository : IGraphRepository
    {
        private readonly GraphApiSettings graphApiSettings;

        public GraphRepository(IOptions<GraphApiSettings> graphApiSettings)
        {
            this.graphApiSettings = graphApiSettings.Value;
        }
        
        public async Task<List<GraphUserDisplayName>> GetUsersDisplayNameAsync(List<string> _)
        {
            var scopes = new string[] {"https://graph.microsoft.com/.default"};
            
            var confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(this.graphApiSettings.ClientId)
                .WithClientSecret(this.graphApiSettings.ClientSecret)
                .WithTenantId(this.graphApiSettings.TenantId)
                .Build();
            
            var graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider(
                async requestMessage =>
                {
                    var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                })
            );

            var req = await graphServiceClient.Users
                .Request()
                .Select("displayName,id")
                .GetAsync();
            var results = req.Select(res => new GraphUserDisplayName(res.Id, res.DisplayName)).ToList();
            return results;
        }
    }
}