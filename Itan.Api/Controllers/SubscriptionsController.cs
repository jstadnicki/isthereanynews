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
        private readonly IMediator _mediator;

        public SubscriptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<SubscribedChannelViewModel>>> Get()
        {
            var userId = Guid.Parse(User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new GetAllSubscribedChannelsViewModelsRequest {PersonId = userId.ToString()};
            var list = await _mediator.Send(command);
            return list;
        }

        [HttpPost]
        [Route("import")]
        public async Task<ActionResult> Post([FromForm(Name = "file")] IFormFile file)
        {

            var userId = Guid.Parse(User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);

            var stream = file.OpenReadStream();
            var httpRequestStreamReader = new HttpRequestStreamReader(stream, Encoding.Default);
            var xmlTextReader = new XmlTextReader(httpRequestStreamReader);
            var xs = new XmlSerializer(typeof(Opml));
            var opml = (Opml) xs.Deserialize(xmlTextReader);

            var command = new ImportSubscriptionsRequest(userId, opml);

            await _mediator.Send(command);
            return Ok();
        }
    }
}