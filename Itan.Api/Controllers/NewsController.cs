using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Itan.Core;
using Itan.Core.GetNewsByChannel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class NewsController : ControllerBase
    {
        private IMediator _mediator;

        public NewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{channelId}")]
        public async Task<ActionResult<List<NewsViewModel>>> Get(Guid channelId)
        {
            var request = new GetNewsByChannelRequest(channelId);
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}