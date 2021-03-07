using System;
using System.Threading;
using System.Threading.Tasks;
using Itan.Common;
using Itan.Core.UpdateShowUpdatedNews;
using MediatR;

namespace Itan.Core.UpdateSquashNewsUpdates
{
    public class UpdateSquashNewsUpdatesRequest : IRequest
    {
        public Guid UserId { get; }
        public SquashUpdate SquashNewsUpdates { get; }

        public UpdateSquashNewsUpdatesRequest(Guid userId, SquashUpdate squashNewsUpdates)
        {
            UserId = userId;
            SquashNewsUpdates = squashNewsUpdates;
        }
    }

    public class UpdateSquashNewsUpdatesRequestHandler : IRequestHandler<UpdateSquashNewsUpdatesRequest, Unit>
    {
        private readonly ISettingsRepository settingsRepository;

        public UpdateSquashNewsUpdatesRequestHandler(ISettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        public async Task<Unit> Handle(UpdateSquashNewsUpdatesRequest request, CancellationToken cancellationToken)
        {
            await this.settingsRepository.UpdateSquashNewsUpdates(request.UserId, request.SquashNewsUpdates);
            return Unit.Value;
        }
    }

   
}

