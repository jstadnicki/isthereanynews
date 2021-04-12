namespace Itan.Core.Requests
{
    public class ChannelCreateRequestResult
    {
        public ChannelCreateRequestResultType ChannelCreateRequestResultType { get; private set; }
        public string ChannelName { get; private set; }

        public static ChannelCreateRequestResult Exists(string title) => new ChannelCreateRequestResult
            {ChannelName = title, ChannelCreateRequestResultType = ChannelCreateRequestResultType.AlreadyExists};

        public static ChannelCreateRequestResult Created(string feedTitle) => new ChannelCreateRequestResult
            {ChannelName = feedTitle, ChannelCreateRequestResultType = ChannelCreateRequestResultType.Created};
    }
}