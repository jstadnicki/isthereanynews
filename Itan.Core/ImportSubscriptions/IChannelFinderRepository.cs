using System;
using System.Threading.Tasks;

namespace Itan.Core.ImportSubscriptions
{
    public interface IChannelFinderRepository
    {
        Task<Guid> FindChannelIdByUrlAsync(string channelUrl, Match match = Match.Exact);

        public enum Match
        {
            Exact,
            Like
        }

    }
}