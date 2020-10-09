using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itan.Core;
using Itan.Core.GetUnreadNewsByChannel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnreadNewsController : ControllerBase
    {
        private IMediator mediator;

        public UnreadNewsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{channelId}")]
        public async Task<ActionResult<List<NewsViewModel>>> Get(string channelId)
        {
            var userId = this.User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var request = new GetUnreadNewsByChannelRequest(channelId, userId);
            var result = await this.mediator.Send(request);
            return this.Ok(result);
        }
    }
}