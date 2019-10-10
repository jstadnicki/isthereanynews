using System;
using System.Collections.Generic;
using Itan.Application;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
