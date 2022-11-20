using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itan.Core;
using Itan.Core.GetNewsByChannel;
using Itan.Core.GetUnreadNewsByChannel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnreadNewsController : ControllerBase
    {
        private IMediator _mediator;

        public UnreadNewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{channelId}")]
        public async Task<ActionResult<List<NewsViewModel>>> Get(string channelId)
        {
            var userId = User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var request = new GetUnreadNewsByChannelRequest(channelId, userId);
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}