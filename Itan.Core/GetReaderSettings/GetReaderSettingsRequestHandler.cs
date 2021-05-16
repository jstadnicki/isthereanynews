using System.Threading;
using System.Threading.Tasks;
using Itan.Core.GetAllSubscribedChannels;
using Itan.Core.GetUnreadNewsByChannel;
using MediatR;

namespace Itan.Core.GetReaderSettings
{
    public class GetReaderSettingsRequestHandler : IRequestHandler<GetReaderSettingsRequest, ReaderSettings>
    {
        private readonly IReaderSettingsRepository repository;

        public GetReaderSettingsRequestHandler(IReaderSettingsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ReaderSettings> Handle(GetReaderSettingsRequest request, CancellationToken cancellationToken) 
            => await this.repository.GetAsync(request.UserId.ToString());
    }
}