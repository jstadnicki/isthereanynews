﻿using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/followers")]
    [ApiController]
    public class FollowersController : ControllerBase
    {
        private IMediator mediator;

        public FollowersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<OkResult> Post(FollowPerson followPersonModel)
        {
            var userId = Guid.Parse(this.User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new FollowPersonCommand(followPersonModel.ReaderId, userId);
            this.mediator.Send(command);
            return Ok();
        }
        
        [HttpDelete]
        [Route("{readerId}")]
        public async Task<OkResult> Delete([FromRoute]UnfollowPerson unfollowPersonModel)
        {
            var userId = Guid.Parse(this.User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new UnfollowPersonCommand(unfollowPersonModel.ReaderId, userId);
            this.mediator.Send(command);
            return Ok();
        }
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