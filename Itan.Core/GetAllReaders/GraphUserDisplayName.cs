namespace Itan.Core.GetAllReaders
{
    public class GraphUserDisplayName
    {
        public string Id { get; }
        public string DisplayName { get; }

        public GraphUserDisplayName(string id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }
    }
}