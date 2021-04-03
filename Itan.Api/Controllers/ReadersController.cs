using System.Collections.Generic;
using System.Threading.Tasks;
using Itan.Core.GetAllReaders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ReadersController : ControllerBase
    {
        private readonly IMediator mediatr;

        public ReadersController(IMediator mediatr)
        {
            this.mediatr = mediatr;
        }

        [HttpGet]
        public async Task<List<ReaderViewModel>> Get()
        {
            var query = new GetAllReadersRequest();
            var result = await this.mediatr.Send(query);
            return result;
        }
    }
}