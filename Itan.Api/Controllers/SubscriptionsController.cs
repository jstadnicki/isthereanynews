using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Itan.Core;
using Itan.Core.GetAllSubscribedChannels;
using Itan.Core.ImportSubscriptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Itan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IMediator mediator;

        public SubscriptionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<List<ChannelViewModel>>> Get(string userId)
        {
            var command = new GetAllSubscribedChannelsViewModelsRequest {PersonId = userId};
            var list = await this.mediator.Send(command);
            return list;
        }

        [HttpPost]
        [Route("import")]
        public async Task<ActionResult> Post([FromForm(Name = "file")] IFormFile file)
        {
            var userId = Guid.Parse(this.User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var stream = file.OpenReadStream();
            var httpRequestStreamReader = new HttpRequestStreamReader(stream, Encoding.Default);
            var xmlTextReader = new XmlTextReader(httpRequestStreamReader);
            var xs = new XmlSerializer(typeof(Opml));
            var opml = (Opml) xs.Deserialize(xmlTextReader);

            var command = new ImportSubscriptionsRequest(userId, opml);

            await this.mediator.Send(command);
            return this.Ok();
        }
    }
}