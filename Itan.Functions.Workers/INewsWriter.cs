using System;
using System.Threading.Tasks;

namespace Itan.Functions.Workers
{
    public interface INewsWriter
    {
        Task InsertNewsLinkAsync(Guid channelId, string title, Guid id);
    }
}