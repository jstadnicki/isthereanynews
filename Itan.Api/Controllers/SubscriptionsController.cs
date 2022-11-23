using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Itan.Core;
using Itan.Core.GetAllSubscribedChannels;
using Itan.Core.ImportSubscriptions;
using Itan.Core.UserSubscribeToChannel;
using Itan.Core.UserUnsubscribeFromChannel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Itan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubscriptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<SubscribedChannelViewModel>>> Get()
        {
            var userId = Guid.Parse(User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new GetAllSubscribedChannelsViewModelsRequest {PersonId = userId.ToString()};
            var list = await _mediator.Send(command);
            return list;
        }

        [HttpPost]
        [Route("channels")]
        public async Task<ActionResult> Post(SubscribeModel model)
        {
            var userId = User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var command = new UserSubscribeToChannelRequest(userId, model.ChannelId);
            await _mediator.Send(command);
            return Accepted();
        }

        [HttpDelete]
        [Route("channels/{channelId}")]
        public async Task<ActionResult> Delete(SubscribeModel model)
        {
            var userId = User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var command = new UserUnsubscribeFromChannelRequest(userId, model.ChannelId);
            await _mediator.Send(command);
            return Accepted();
        }

        public class SubscribeModel
        {
            public string ChannelId { get; set; }
        }
    }
}