using System.Threading.Tasks;

namespace Itan.Functions.Workers
{
    public interface IFunctionWorker
    {
        Task Run();
    }
}