using System.Threading.Tasks;

namespace Itan.Functions.Workers;

public interface IFunction5Worker
{
    Task Run(string myQueueItem);
}