using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Core.GetAllReaders
{
    public class GetAllReadersRequestHandler : IRequestHandler<GetAllReadersRequest, List<ReaderViewModel>>
    {
        private readonly IReadersRepository _readersRepository;
        private readonly IGraphRepository _graphRepository;

        public GetAllReadersRequestHandler(
            IReadersRepository readersRepository, 
            IGraphRepository graphRepository)
        {
            _readersRepository = readersRepository;
            _graphRepository = graphRepository;
        }

        public async Task<List<ReaderViewModel>> Handle(GetAllReadersRequest request, CancellationToken cancellationToken)
        {
            var readersIds = await _readersRepository.GetAllIdsAsync();
            var guids = readersIds.Select(x => x.Id.ToString()).ToList();
            var usersDisplayNameAsync = await _graphRepository.GetUsersDisplayNameAsync(guids);
            
            var viewModels = usersDisplayNameAsync
                .Where(gvm=>guids.Contains(gvm.Id))
                .Select(vm => new ReaderViewModel(vm.Id, vm.DisplayName)).ToList();
            return viewModels;
        }
    }
}