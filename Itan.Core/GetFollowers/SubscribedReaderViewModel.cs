namespace Itan.Core.GetFollowers
{
    public class SubscribedReaderViewModel
    {
        public string PersonId { get; }
        public string DisplayName { get; }

        public SubscribedReaderViewModel(string personId, string displayName)
        {
            PersonId = personId;
            DisplayName = displayName;
        }
    }
}