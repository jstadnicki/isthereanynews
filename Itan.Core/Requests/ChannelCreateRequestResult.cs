namespace Itan.Core.Requests
{
    public enum ChannelCreateRequestResult
    {
        Unknown = 0,
        Created,
        AlreadyExists,
        NotValidUrl,
        NoResponse
    }
}