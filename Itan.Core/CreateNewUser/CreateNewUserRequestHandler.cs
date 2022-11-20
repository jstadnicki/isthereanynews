using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Itan.Core.DeleteAccount;
using Itan.Core.GetAllSubscribedChannels;
using Itan.Core.GetUnreadNewsByChannel;
using Itan.Wrappers;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace Itan.Core.CreateNewUser
{
    public class CreateNewUserRequestHandler : IRequestHandler<CreateNewUserRequest>
    {
        private readonly ICreateUserRepository _repository;
        private readonly GraphApiSettings _graphApiSettings;
        private readonly IEmailSender _emailWrapper;
        private readonly IReaderSettingsRepository _readersSettingsRepository;

        public CreateNewUserRequestHandler(ICreateUserRepository repository,
            IOptions<GraphApiSettings> graphApiSettings,
            IEmailSender emailWrapper,
            IReaderSettingsRepository readersSettingsRepository)
        {
            _repository = repository;
            _emailWrapper = emailWrapper;
            _readersSettingsRepository = readersSettingsRepository;
            _graphApiSettings = graphApiSettings.Value;
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
            Validate(request);
            await _repository.CreatePersonIfNotExists(request.UserId);
            await _readersSettingsRepository.CreateDefaultValuesAsync(request.UserId);

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

            var user = await graphServiceClient.Users[request.UserId.ToString()].Request().GetAsync(cancellationToken);
            await _emailWrapper.SendEmailNewAccountRegisteredAsync(user.DisplayName, request.UserId);

            return await Task.FromResult(new Unit());
        }
    }
}