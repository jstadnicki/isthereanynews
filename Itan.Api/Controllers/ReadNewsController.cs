using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itan.Core.MarkNewsAsReadWithClick;
using Itan.Core.MarkNewsRead;
using Itan.Core.MarkUnreadNewsAsRead;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChannelReadNewsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ChannelReadNewsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]MarkNewsAsReadRequest request)
        {
            var userId = Guid.Parse(this.User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new MarkNewsReadCommand(request.ChannelId, request.NewsId, userId);
            await this.mediator.Send(command);
            return this.Ok();
        }
        


        [HttpPost]
        [Authorize]
        [Route("skipped")]
        public async Task<ActionResult> Post([FromBody]MarkUnreadNewsAsReadRequest request)
        {
            var userId = Guid.Parse(this.User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new MarkUnreadNewsAsReadCommand(request.ChannelId, request.NewsId, userId);
            await this.mediator.Send(command);
            return this.Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("click")]
        public async Task<ActionResult> Post([FromBody]MarkNewsAsReadWithClickRequest request)
        {
            var userId = Guid.Parse(this.User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new MarkNewsAsReadWithClickCommand(request.ChannelId, request.NewsId, userId);
            await this.mediator.Send(command);
            return this.Ok();
        }

        public class MarkNewsAsReadRequest
        {
            public Guid ChannelId { get; set; }
            public Guid NewsId { get; set; }
        }

        public class MarkNewsAsReadWithClickRequest
        {
            public Guid ChannelId { get; set; }
            public Guid NewsId { get; set; }
        }

        public class MarkUnreadNewsAsReadRequest
        {
            public Guid ChannelId { get; set; }
            public ICollection<Guid> NewsId { get; set; }
        }
    }
}