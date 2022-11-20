using System;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Itan.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Itan.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly IOptions<ConnectionOptions> _options;

        public HealthController(IOptions<ConnectionOptions> options)
        {
            _options = options;
        }
        
        [Route("ping")]
        public ActionResult<string> Ping()
        {
            return Ok("Pong");
        }
        
        [Route("akv")]
        public async Task<ActionResult<string>> Akv()
        {
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            };


            var defaultAzureCredential = new DefaultAzureCredential();
            var client = new SecretClient(new Uri("https://itan-kv-secrets.vault.azure.net"), defaultAzureCredential);

            // await client.SetSecretAsync("code-secret", "42");
            KeyVaultSecret secret =await  client.GetSecretAsync("itan-test-secret");

            string secretValue = secret.Value;

            return secretValue;
        }
    }
}