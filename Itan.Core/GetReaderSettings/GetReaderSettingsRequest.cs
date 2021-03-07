using System;
using System.Threading;
using System.Threading.Tasks;
using Itan.Core.GetUnreadNewsByChannel;
using MediatR;

namespace Itan.Core.GetReaderSettings
{
    public class GetReaderSettingsRequest : IRequest<ReaderSettings>
    {
        public Guid UserId { get; }

        public GetReaderSettingsRequest(Guid userId)
        {
            UserId = userId;
        }
    }
    
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