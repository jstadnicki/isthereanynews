using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Core.UpdateShowUpdatedNews
{
    internal class UpdateShowUpdatedNewsRequestHandler : IRequestHandler<UpdateShowUpdatedNewsRequest, Unit>
    {
        private readonly ISettingsRepository settingsRepository;

        public UpdateShowUpdatedNewsRequestHandler(ISettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        public async Task<Unit> Handle(UpdateShowUpdatedNewsRequest request, CancellationToken cancellationToken)
        {
            await this.settingsRepository.UpdateShowUpdatedNews(request.UserId, request.ShowUpdatedNews);
            return Unit.Value;
        }
    }
}