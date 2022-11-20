using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itan.Api.Dto;
using Itan.Core;
using Itan.Core.ChannelsCreateNewChannel;
using Itan.Core.GetAllChannels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ChannelsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChannelsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ChannelViewModel>>> Get()
        {
            var command = new GetAllChannelsViewModelsRequest();
            var list = await _mediator.Send(command);
            return list;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] ChannelsPostDto model)
        {
            var userId = User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var command = new ChannelsCreateNewChannelRequest
            {
                Url = model.Url, PersonId = Guid.Parse(userId)
            };
            var channelCreateRequestResult = await _mediator.Send(command);
            return Accepted(channelCreateRequestResult);
        }
    }
}