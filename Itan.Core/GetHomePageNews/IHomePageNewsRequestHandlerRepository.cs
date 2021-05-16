using System.Threading.Tasks;

namespace Itan.Core.GetHomePageNews
{
    public interface IHomePageNewsRequestHandlerRepository
    {
        Task<HomePageNewsViewModel> GetHomePageNews();
    }
}