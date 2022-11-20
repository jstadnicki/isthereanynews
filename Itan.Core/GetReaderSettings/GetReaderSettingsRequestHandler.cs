using System.Threading;
using System.Threading.Tasks;
using Itan.Core.GetAllSubscribedChannels;
using Itan.Core.GetUnreadNewsByChannel;
using MediatR;

namespace Itan.Core.GetReaderSettings
{
    public class GetReaderSettingsRequestHandler : IRequestHandler<GetReaderSettingsRequest, ReaderSettings>
    {
        private readonly IReaderSettingsRepository _repository;

        public GetReaderSettingsRequestHandler(IReaderSettingsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ReaderSettings> Handle(GetReaderSettingsRequest request, CancellationToken cancellationToken) 
            => await _repository.GetAsync(request.UserId.ToString());
    }
}