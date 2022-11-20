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
        private readonly GraphApiSettings _graphApiSettings;

        public DeleteAccountCommandHandler(IOptions<GraphApiSettings> graphApiSettings)
        {
            _graphApiSettings = graphApiSettings.Value;
        }
        public async Task<Unit> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var scopes = new string[] {"https://graph.microsoft.com/.default"};
            
            var confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(_graphApiSettings.ClientId)
                .WithClientSecret(_graphApiSettings.ClientSecret)
                .WithTenantId(_graphApiSettings.TenantId)
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