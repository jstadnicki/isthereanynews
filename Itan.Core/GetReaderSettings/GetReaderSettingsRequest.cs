using System;
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
}