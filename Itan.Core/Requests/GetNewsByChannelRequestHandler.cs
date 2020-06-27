using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Core.Requests
{
    public class GetNewsByChannelRequestHandler : IRequestHandler<GetNewsByChannelRequest, List<NewsViewModel>>
    {
        public Task<List<NewsViewModel>> Handle(GetNewsByChannelRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}