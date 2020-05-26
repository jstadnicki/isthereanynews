using System.Collections.Generic;
using System.Threading.Tasks;
using Itan.Core;
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
        private readonly IMediator mediator;

        public ChannelsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ChannelViewModel>>> Get()
        {
            var command = new GetAllChannelsViewModelsRequest();
            var list = await this.mediator.Send(command);
            return list;
        }
    }
}
