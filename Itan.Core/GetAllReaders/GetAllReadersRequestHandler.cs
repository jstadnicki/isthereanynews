using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Itan.Core.GetAllReaders
{
    public class GetAllReadersRequestHandler : IRequestHandler<GetAllReadersRequest, List<ReaderViewModel>>
    {
        private readonly IReadersRepository readersRepository;
        private readonly IGraphRepository graphRepository;

        public GetAllReadersRequestHandler(
            IReadersRepository readersRepository, 
            IGraphRepository graphRepository)
        {
            this.readersRepository = readersRepository;
            this.graphRepository = graphRepository;
        }

        public async Task<List<ReaderViewModel>> Handle(GetAllReadersRequest request, CancellationToken cancellationToken)
        {
            var readersIds = await this.readersRepository.GetAllIdsAsync();
            var guids = readersIds.Select(x => x.Id.ToString()).ToList();
            var usersDisplayNameAsync = await this.graphRepository.GetUsersDisplayNameAsync(guids);
            
            var viewModels = usersDisplayNameAsync
                .Where(gvm=>guids.Contains(gvm.Id))
                .Select(vm => new ReaderViewModel(vm.Id, vm.DisplayName)).ToList();
            return viewModels;
        }
    }
}