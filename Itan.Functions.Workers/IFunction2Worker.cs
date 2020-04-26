using System.Threading.Tasks;

namespace Itan.Functions.Workers
{
    public interface IFunction2Worker
    {
        Task Run(string queueItem);
    }
}