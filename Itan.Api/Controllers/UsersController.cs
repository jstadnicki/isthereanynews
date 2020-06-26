using System.Threading.Tasks;
using Itan.Api.Dto;
using Itan.Core.CreateNewUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Post(UsersControllerPostDto dto)
        {
            var command = new CreateNewUserRequest(dto.UserId);
            await this.mediator.Send(command);
            return this.Accepted();
        }
    }
}