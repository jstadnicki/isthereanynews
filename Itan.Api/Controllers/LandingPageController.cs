using System.Threading.Tasks;
using Itan.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Itan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LandingPageController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public LandingPageController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        [Route("news")]
        [HttpGet]
        public async Task<ActionResult<HomePageNews>> Get()
        {
            var np = new NewsProvider(configuration);
            var hpn = await np.GetHomePageNews();
            return this.Ok(hpn);
        }
    }
}