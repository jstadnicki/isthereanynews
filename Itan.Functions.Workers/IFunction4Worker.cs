using System.Threading.Tasks;

namespace Itan.Functions.Workers
{
    public interface IFunction4Worker
    {
        Task RunAsync(string myQueueItem);
    }
}