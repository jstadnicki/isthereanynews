using System.Threading.Tasks;
using Itan.Core.GetHomePageNews;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LandingPageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LandingPageController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [Route("news")]
        [HttpGet]
        public async Task<ActionResult<HomePageNewsViewModel>> Get()
        {
            var request = new GetHomePageNewsRequest();
            var hpn = await _mediator.Send(request);
            return Ok(hpn);
        }
    }
}