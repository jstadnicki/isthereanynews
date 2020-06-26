using System.Threading.Tasks;
using Itan.Api.Dto;
using Itan.Core.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserChannelsSubscriptionsController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserChannelsSubscriptionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("{userId}/channels")]
        public async Task<ActionResult> Post(string userId, UserChannelsSubscriptionsControllerPostDto model)
        {
            var command = new UserSubscribeToChannelRequest(userId, model.ChannelId);
            await this.mediator.Send(command);
            return this.Accepted();
        }

        [HttpDelete]
        [Route("{userId}/channels/{channelId}")]
        public async Task<ActionResult> Delete(string userId, string channelId)
        {
            var command = new UserUnsubscribeFromChannelRequest(userId, channelId);
            await this.mediator.Send(command);
            return this.Accepted();
        }
    }
}