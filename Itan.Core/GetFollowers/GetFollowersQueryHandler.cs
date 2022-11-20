using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Itan.Common;
using Itan.Core.DeleteAccount;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace Itan.Core.GetFollowers
{
    public class GetFollowersQueryHandler : IRequestHandler<GetFollowersQuery, List<SubscribedReaderViewModel>>
    {
        private readonly GraphApiSettings _graphApiSettings;
        private readonly string _readConnectionString;

        public GetFollowersQueryHandler(
            IOptions<ConnectionOptions> options, 
            IOptions<GraphApiSettings> graphApiSettings)
        {
            _graphApiSettings = graphApiSettings.Value;
            _readConnectionString = options.Value.SqlReader;
        }

        public async Task<List<SubscribedReaderViewModel>> Handle(GetFollowersQuery request, CancellationToken cancellationToken)
        {
            var followersIds = await GetFollowersIds(request);

            if (!followersIds.Any())
            {
                return new List<SubscribedReaderViewModel>();
            }

            var confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(_graphApiSettings.ClientId)
                .WithClientSecret(_graphApiSettings.ClientSecret)
                .WithTenantId(_graphApiSettings.TenantId)
                .Build();
            
            var scopes = new string[] {"https://graph.microsoft.com/.default"};
            
            var graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider(
                async requestMessage =>
                {
                    var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                })
            );

            var quotedIds = followersIds.Select(x => $"'{x}'");
            var filter = "id eq '" + followersIds.First()+"'";
            if (quotedIds.Count() > 1)
            {
                var ids = string.Join(string.Empty,quotedIds.Skip(1).Select(x => $" or id eq {x}"));
                filter+=ids;
            }

            var req = await graphServiceClient.Users
                .Request()
                .Select("displayName,id")
                .Filter(filter)
                .GetAsync();
            
            var results = req.Select(res => new SubscribedReaderViewModel(res.Id, res.DisplayName)).ToList();
            return results;
        }

        private async Task<List<Guid>> GetFollowersIds(GetFollowersQuery request)
        {
            var query = "SELECT pp.TargetPersonId from PersonsPersons pp\n" +
                        "WHERE pp.FollowerPersonId=@followerPersonId";
            var queryData = new
            {
                followerPersonId = request.FollowerPersonId
            };

            await using var connection = new SqlConnection(_readConnectionString);
            var queryAsyncRaw = await connection.QueryAsync<Guid>(query, queryData);
            var followersIds = queryAsyncRaw.ToList();
            return followersIds;
        }
    }
}