using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Itan.Core;
using Itan.Core.GetNewsByChannel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Itan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class NewsController : ControllerBase
    {
        private IMediator mediator;

        public NewsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{channelId}")]
        public async Task<ActionResult<List<NewsViewModel>>> Get(Guid channelId)
        {
            var request = new GetNewsByChannelRequest(channelId);
            var result = await this.mediator.Send(request);
            return this.Ok(result);
        }
    }
}