using System;
using System.Linq;
using System.Threading.Tasks;
using Itan.Api.Dto;
using Itan.Core.CreateNewUser;
using Itan.Core.MigrateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Post(UsersControllerPostDto dto)
        {
            var command = new CreateNewUserRequest(dto.UserId);
            await _mediator.Send(command);
            return Accepted();
        }
        
        [HttpPost]
        [Route("migrate")]
        [Authorize]
        public async Task<ActionResult> Post()
        {
            var userId = Guid.Parse(User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new MigrateUserRequest(userId);
            await _mediator.Send(command);
            return Ok();
        }
    }
}