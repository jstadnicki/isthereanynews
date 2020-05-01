using System.Threading.Tasks;
using Itan.Functions.Models;

namespace Itan.Functions.Workers
{
    public interface IChannelUpdater
    {
        Task Update(ChannelUpdate message);
    }
}