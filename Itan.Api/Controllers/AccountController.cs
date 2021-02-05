using System;
using System.Linq;
using System.Threading.Tasks;
using Itan.Core.DeleteAccount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController:ControllerBase
    {
        private readonly IMediator mediator;

        public AccountController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<OkResult> Delete()
        {
            var userId = Guid.Parse(this.User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new DeleteAccountCommand(userId);
            await this.mediator.Send(command);
            return this.Ok();
        }
    }
}