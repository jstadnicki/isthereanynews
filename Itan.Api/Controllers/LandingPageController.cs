using System.Threading.Tasks;
using Itan.Core;
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
        private readonly IMediator mediator;

        public LandingPageController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        [Route("news")]
        [HttpGet]
        public async Task<ActionResult<HomePageNewsViewModel>> Get()
        {
            var request = new GetHomePageNewsRequest();
            var hpn = await this.mediator.Send(request);
            return this.Ok(hpn);
        }
    }
}