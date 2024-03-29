﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Itan.Common;
using Itan.Core.GetReaderSettings;
using Itan.Core.UpdateShowUpdatedNews;
using Itan.Core.UpdateSquashNewsUpdates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Itan.Api.Controllers
{
    [Route("api/settings")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public SettingsController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpPatch]
        [Route("show-updated-news")]
        public async Task<IActionResult> PatchShowUpdatedNews(PatchShowUpdatedNewsModel model)
        {
            var userId = Guid.Parse(User.Claims
                .Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new UpdateShowUpdatedNewsRequest(userId, model.ShowUpdatedNews);
            await _mediatr.Send(command);
            return Accepted();
        }

        [HttpPatch]
        [Route("squash-news-updates")]
        public async Task<IActionResult> PatchSquashNewsUpdates(PatchSquashUpdateModel model)
        {
            var userId = Guid.Parse(User.Claims
                .Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new UpdateSquashNewsUpdatesRequest(userId, model.SquashUpdate);
            await _mediatr.Send(command);
            return Accepted();
        }

        [HttpGet]
        [Route("reader")]
        public async Task<IActionResult> Reader()
        {
            var userId = Guid.Parse(User.Claims.Single(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value);
            var command = new GetReaderSettingsRequest(userId);
            var readerSettings = await _mediatr.Send(command);
            return Ok(readerSettings);
        }
    }

    public class PatchShowUpdatedNewsModel
    {
        public UpdatedNews ShowUpdatedNews { get; set; }
    }

    public class PatchSquashUpdateModel
    {
        public SquashUpdate SquashUpdate { get; set; }
    }
}