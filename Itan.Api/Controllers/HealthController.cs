using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        public ActionResult<string> Ping()
        {
            return this.Ok("Pong");
        }
    }
}