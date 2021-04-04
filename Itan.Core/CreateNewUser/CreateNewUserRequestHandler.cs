using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Itan.Core.DeleteAccount;
using Itan.Core.GetUnreadNewsByChannel;
using Itan.Core.Handlers;
using Itan.Wrappers;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace Itan.Core.CreateNewUser
{
    public class CreateNewUserRequestHandler : IRequestHandler<CreateNewUserRequest>
    {
        private readonly ICreateUserRepository repository;
        private readonly GraphApiSettings graphApiSettings;
        private readonly IEmailSender emailWrapper;
        private readonly IReaderSettingsRepository readersSettingsRepository;

        public CreateNewUserRequestHandler(ICreateUserRepository repository,
            IOptions<GraphApiSettings> graphApiSettings,
            IEmailSender emailWrapper, 
            IReaderSettingsRepository readersSettingsRepository)
        {
            this.repository = repository;
            this.emailWrapper = emailWrapper;
            this.readersSettingsRepository = readersSettingsRepository;
            this.graphApiSettings = graphApiSettings.Value;
        }
        private void Validate(CreateNewUserRequest request)
        {
            if (Guid.Empty == request.UserId)
            {
                throw new BadArgumentInRequestException(nameof(CreateNewUserRequestHandler), nameof(request.UserId));
            }
        }

        public async Task<Unit> Handle(CreateNewUserRequest request, CancellationToken cancellationToken)
        {
            this.Validate(request);
            await this.repository.CreatePersonIfNotExists(request.UserId);
            await this.readersSettingsRepository.CreateDefaultValuesAsync(request.UserId);
            
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

            // var user = await graphServiceClient.Users[request.UserId.ToString()].Request().GetAsync(cancellationToken);
            // await this.emailWrapper.SendEmailNewAccountRegisteredAsync(user.DisplayName, request.UserId);
            
            return await Task.FromResult(new Unit());
        }
    }
}