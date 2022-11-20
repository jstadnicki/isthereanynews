using System;
using System.Linq;
using System.Threading.Tasks;
using Itan.Core.DeleteAccount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/delete-account")]
    [ApiController]
    public class AccountController:ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete]
        public async Task<OkResult> Delete()
        {
            var userId = Guid.Parse(User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new DeleteAccountCommand(userId);
            await _mediator.Send(command);
            return Ok();
        }
    }
}