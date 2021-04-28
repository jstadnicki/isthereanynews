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
        private readonly EmailSenderSettings emailSenderSettings;

        public EmailSender(IOptions<EmailSenderSettings> emailSenderOptions)
        {
            this.emailSenderSettings = emailSenderOptions.Value;
        }

        public async Task SendEmailNewAccountRegisteredAsync(string userDisplayName, Guid userId)
        {
            var plainTextContent = $"New registration! {userDisplayName} has just registered, find him by this id: {userId}";
            var client = new MailjetClient(emailSenderSettings.ApiKey, emailSenderSettings.ApiSecret);
            var request = new MailjetRequest
                {
                    Resource = Send.Resource,
                }
                .Property(Send.FromEmail, emailSenderSettings.From)
                .Property(Send.FromName, "ITAN contact")
                .Property(Send.Subject, "New registration!")
                .Property(Send.TextPart, plainTextContent)
                .Property(Send.Recipients, new JArray
                {
                    new JObject
                    {
                        {"Email", emailSenderSettings.To}
                    }
                });
            await client.PostAsync(request);
        }
    }
}