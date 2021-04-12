using System.Threading;
using System.Threading.Tasks;
using Itan.Core.UpdateShowUpdatedNews;
using MediatR;

namespace Itan.Core.UpdateSquashNewsUpdates
{
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