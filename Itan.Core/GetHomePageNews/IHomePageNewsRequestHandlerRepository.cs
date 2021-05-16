using System.Threading.Tasks;

namespace Itan.Core.Requests
{
    public interface IHomePageNewsRequestHandlerRepository
    {
        Task<HomePageNewsViewModel> GetHomePageNews();
    }
}