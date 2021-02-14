using System;
using System.Threading.Tasks;

namespace Itan.Wrappers
{
    public interface IEmailSender
    {
        Task SendEmailNewAccountRegisteredAsync(string userDisplayName, Guid userId);
    }
}