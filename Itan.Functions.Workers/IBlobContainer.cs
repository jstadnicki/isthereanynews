using System;
using System.Threading.Tasks;

namespace Itan.Functions.Workers
{
    public interface IBlobContainer
    {
        Task UploadTextAsync(string id, string channelString);
    }
}