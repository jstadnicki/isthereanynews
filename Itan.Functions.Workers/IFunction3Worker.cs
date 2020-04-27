using System;
using System.IO;
using System.Threading.Tasks;

namespace Itan.Functions.Workers
{
    public interface IFunction3Worker
    {
        Task RunAsync(Guid channelId, string blobName, Stream myBlob);
    }
}