using System.Threading.Tasks;
using Itan.Api.Dto;
using Itan.Core.UserSubscribeToChannel;
using Itan.Core.UserUnsubscribeFromChannel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserChannelsSubscriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserChannelsSubscriptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("{userId}/channels")]
        public async Task<ActionResult> Post(string userId, UserChannelsSubscriptionsControllerPostDto model)
        {
            var command = new UserSubscribeToChannelRequest(userId, model.ChannelId);
            await _mediator.Send(command);
            return Accepted();
        }

        [HttpDelete]
        [Route("{userId}/channels/{channelId}")]
        public async Task<ActionResult> Delete(string userId, string channelId)
        {
            var command = new UserUnsubscribeFromChannelRequest(userId, channelId);
            await _mediator.Send(command);
            return Accepted();
        }
    }
}