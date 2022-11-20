using System;
using System.Linq;
using System.Threading.Tasks;
using Itan.Core.MarkNewsOpen;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelOpenNewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChannelOpenNewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody]MarkNewsAsOpenRequest request)
        {
            var userId = Guid.Parse(User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new MarkNewsOpenCommand(request.ChannelId, request.NewsId, userId);
            await _mediator.Send(command);
            return Ok();
        }

        public class MarkNewsAsOpenRequest
        {
            public Guid ChannelId { get; set; }
            public Guid NewsId { get; set; }
        }
    }
}