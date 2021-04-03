using System.Collections.Generic;
using MediatR;

namespace Itan.Core.GetAllReaders
{
    public class GetAllReadersRequest : IRequest<List<ReaderViewModel>>
    {
    }
}