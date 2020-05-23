using System.Collections.Generic;
using Itan.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ChannelsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<ChannelViewModel>> Get()
        {
            var x = new ChannelsProvider();
            return x.GetAll();
        }
    }
}
