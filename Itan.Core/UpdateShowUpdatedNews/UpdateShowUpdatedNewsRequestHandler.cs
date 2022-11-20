using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Core.UpdateShowUpdatedNews
{
    internal class UpdateShowUpdatedNewsRequestHandler : IRequestHandler<UpdateShowUpdatedNewsRequest, Unit>
    {
        private readonly ISettingsRepository _settingsRepository;

        public UpdateShowUpdatedNewsRequestHandler(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<Unit> Handle(UpdateShowUpdatedNewsRequest request, CancellationToken cancellationToken)
        {
            await _settingsRepository.UpdateShowUpdatedNews(request.UserId, request.ShowUpdatedNews);
            return Unit.Value;
        }
    }
}