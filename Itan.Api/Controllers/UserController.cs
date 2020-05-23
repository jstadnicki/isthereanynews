using System.Threading.Tasks;
using Itan.Core.CreateNewUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Post(string userId)
        {
            var command = new CreateNewUserRequest(userId);
            await this.mediator.Send(command);
            return this.Accepted();
        }
    }
}