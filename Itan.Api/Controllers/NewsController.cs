using System;
using System.Collections.Generic;
using Itan.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Itan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public NewsController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        [Route("{channelId}")]
        public ActionResult<List<NewsViewModel>> Get(Guid channelId)
        {
            var x = new NewsProvider(this.configuration);
            return x.GetAllByChannelId(channelId);
        }
    }
}