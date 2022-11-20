using System.Collections.Generic;
using System.Threading.Tasks;
using Itan.Core.GetAllReaders;
using Itan.Core.GetReader;
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
        private readonly IMediator _mediatr;

        public ReadersController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpGet]
        public async Task<List<ReaderViewModel>> Get()
        {
            var query = new GetAllReadersRequest();
            var result = await _mediatr.Send(query);
            return result;
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<ReaderDetailsViewModel> Get(string id)
        {
            var query = new GetReaderRequest(id);
            var result = await _mediatr.Send(query);
            return result;
        }
    }
}