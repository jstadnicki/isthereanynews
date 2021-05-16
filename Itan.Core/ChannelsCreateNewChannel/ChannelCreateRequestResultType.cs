namespace Itan.Core.ChannelsCreateNewChannel
{
    public enum ChannelCreateRequestResultType
    {
        Unknown = 0,
        Created,
        AlreadyExists,
        NotValidUrl,
        NoResponse
    }
}