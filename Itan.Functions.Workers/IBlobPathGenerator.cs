using System;

namespace Itan.Functions.Workers
{
    public interface IBlobPathGenerator
    {
        string GetChannelDownloadPath(Guid id);
    }
}