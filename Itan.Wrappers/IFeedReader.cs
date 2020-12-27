namespace Itan.Wrappers
{
    public interface IFeedReader
    {
        ItanFeed GetFeed(string feedString);
    }
}