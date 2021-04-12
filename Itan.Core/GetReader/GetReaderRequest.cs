using MediatR;

namespace Itan.Core.GetReader
{
    public class GetReaderRequest : IRequest<ReaderDetailsViewModel>
    {
        public string Id { get; }

        public GetReaderRequest(string id)
        {
            Id = id;
        }
    }
}