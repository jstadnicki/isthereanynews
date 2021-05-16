using System.Threading.Tasks;
using Itan.Common;

namespace Itan.Functions.Workers
{
    public interface IChannelUpdater
    {
        Task Update(ChannelUpdate message);
    }
}