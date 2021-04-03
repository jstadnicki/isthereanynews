namespace Itan.Core.GetAllReaders
{
    public class ReaderViewModel
    {
        public ReaderViewModel(string id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }

        public string Id { get;  }
        public string DisplayName { get; }
    }
}