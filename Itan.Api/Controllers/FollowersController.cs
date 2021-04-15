using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/followers")]
    [ApiController]
    public class FollowersController : ControllerBase
    {
        [HttpPost]
        public async Task<OkResult> Post(FollowPerson followPersonModel)
        {
            return this.Ok();
        }
        
        [HttpDelete]
        [Route("{readerId}")]
        public async Task<OkResult> Delete([FromRoute]UnfollowPerson unfollowPersonModel)
        {
            return this.Ok();
        }
    }

    public class UnfollowPerson
    {
        public string ReaderId { get; set; }
    }

    public class FollowPerson
    {
        public string ReaderId { get; set; }
    }
}