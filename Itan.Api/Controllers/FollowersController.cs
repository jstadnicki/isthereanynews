using System;
using System.Linq;
using System.Threading.Tasks;
using Itan.Core.FollowPerson;
using Itan.Core.GetFollowerActivity;
using Itan.Core.GetFollowers;
using Itan.Core.UnfollowPerson;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/followers")]
    [ApiController]
    public class FollowersController : ControllerBase
    {
        private IMediator _mediator;

        public FollowersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<OkResult> Post([FromBody]FollowPerson followPersonModel)
        {
            var userId = Guid.Parse(User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new FollowPersonCommand(followPersonModel.ReaderId, userId);
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpDelete]
        [Route("{readerId}")]
        public async Task<OkResult> Delete([FromRoute]UnfollowPerson unfollowPersonModel)
        {
            var userId = Guid.Parse(User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new UnfollowPersonCommand(unfollowPersonModel.ReaderId, userId);
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = Guid.Parse(User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new GetFollowersQuery(userId);
            var followers = await _mediator.Send(command);
            return Ok(followers);
        }
        
        [HttpGet]
        [Route("{personId}/activity")]
        public async Task<IActionResult> Get([FromRoute]PersonActivity model)
        {
            var userId = Guid.Parse(User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new GetFollowerActivityQuery(model.PersonId);
            var followers = await _mediator.Send(command);
            return Ok(followers);
        }
    }

    public class PersonActivity
    {
        public string PersonId { get; set; }
    }

    public class UnfollowPerson
    {
        public string ReaderId { get; set; }
    }

    public class FollowPerson
    {
        public string ReaderId { get; set; }
    }
}