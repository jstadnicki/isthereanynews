namespace Itan.Core.Requests
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