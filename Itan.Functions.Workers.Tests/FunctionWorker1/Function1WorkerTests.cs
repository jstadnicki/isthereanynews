// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using AutoFixture.Xunit2;
// using Itan.Functions.Models;
// using Moq;
// using Xunit;
//
// namespace Itan.Functions.Workers.Tests.FunctionWorker1
// {
//     public class Function1WorkerTests
//     {
//         [Theory, AutoData]
//         public async Task AllReadChannelsAreBeingSendToQueue(Function1WorkerFixture workerFixture, int numberOfChannels)
//         {
//             // arrange
//             var worker = workerFixture
//                 .CreateChannelsToDownloads(numberOfChannels)
//                 .GetWorker();
//         
//             // act
//             await worker.Run();
//         
//             // assert
//             workerFixture
//                 .QueueMock
//                 .Verify(v=>v.AddRangeAsync(It.Is<IEnumerable<ChannelToDownload>>(p=>p.Count() == numberOfChannels)));
//         }    
//     }
// }