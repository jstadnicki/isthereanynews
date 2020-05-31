using System.Collections.Generic;
using System.Threading.Tasks;
using Itan.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IMediator mediator;

        public SubscriptionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<List<ChannelViewModel>>> Get(string userId)
        {
            var command = new GetAllSubscribedChannelsViewModelsRequest {PersonId = userId};
            var list = await this.mediator.Send(command);
            return list;
        }
    }
}