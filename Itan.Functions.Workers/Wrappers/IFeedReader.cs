using Itan.Functions.Workers.Model;

namespace Itan.Functions.Workers.Wrappers
{
    public interface IFeedReader
    {
        ItanFeed GetFeed(string feedString);
    }
}