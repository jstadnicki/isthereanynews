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
    }
}