using System;
using Itan.Common;
using MediatR;

namespace Itan.Core.UpdateShowUpdatedNews
{
    public class UpdateShowUpdatedNewsRequest : IRequest
    {
        public UpdateShowUpdatedNewsRequest(Guid userId, UpdatedNews showUpdatedNews)
        {
            UserId = userId;
            ShowUpdatedNews = showUpdatedNews;
        }

        public UpdatedNews ShowUpdatedNews { get; }

        public Guid UserId { get; }
    }
}