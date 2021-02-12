using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace Itan.Core.DeleteAccount
{
    public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Unit>
    {
        private readonly GraphApiSettings graphApiSettings;

        public DeleteAccountCommandHandler(IOptions<GraphApiSettings> graphApiSettings)
        {
            this.graphApiSettings = graphApiSettings.Value;
        }
        public async Task<Unit> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
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
                    var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync(cancellationToken);
                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                })
            );

            await graphServiceClient.Users[request.UserId.ToString()].Request().DeleteAsync(cancellationToken);
            await graphServiceClient.Directory.DeletedItems[request.UserId.ToString()].Request().DeleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}