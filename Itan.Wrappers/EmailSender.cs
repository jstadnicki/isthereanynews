using System;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Itan.Wrappers
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderSettings sendGridSettings;
        public EmailSender(IOptions<EmailSenderSettings> sendGridOptions)
        {
            this.sendGridSettings = sendGridOptions.Value;
        }

        public async Task SendEmailNewAccountRegisteredAsync(string userDisplayName, Guid userId)
        {
            var plainTextContent = $"New registration! {userDisplayName} has just registered, find him by this id: {userId}";

            var x = new SmtpClient(sendGridSettings.SmtpServer, sendGridSettings.SmtpPort);
            x.UseDefaultCredentials = false;
            x.Credentials = new NetworkCredential(sendGridSettings.UserName, sendGridSettings.Password);
            x.EnableSsl = true;
            x.DeliveryMethod = SmtpDeliveryMethod.Network;
            var mm = new MailMessage(sendGridSettings.From, sendGridSettings.To, "New registration", plainTextContent);
            await x.SendMailAsync(mm);
        }
    }
}