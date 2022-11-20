using System;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Itan.Wrappers
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderSettings _emailSenderSettings;

        public EmailSender(IOptions<EmailSenderSettings> emailSenderOptions)
        {
            _emailSenderSettings = emailSenderOptions.Value;
        }

        public async Task SendEmailNewAccountRegisteredAsync(string userDisplayName, Guid userId)
        {
            var plainTextContent = $"New registration! {userDisplayName} has just registered, find him by this id: {userId}";
            var client = new MailjetClient(_emailSenderSettings.ApiKey, _emailSenderSettings.ApiSecret);
            var request = new MailjetRequest
                {
                    Resource = Send.Resource,
                }
                .Property(Send.FromEmail, _emailSenderSettings.From)
                .Property(Send.FromName, "ITAN contact")
                .Property(Send.Subject, "New registration!")
                .Property(Send.TextPart, plainTextContent)
                .Property(Send.Recipients, new JArray
                {
                    new JObject
                    {
                        {"Email", _emailSenderSettings.To}
                    }
                });
            await client.PostAsync(request);
        }
    }
}