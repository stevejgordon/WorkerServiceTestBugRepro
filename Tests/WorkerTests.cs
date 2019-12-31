using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Threading.Tasks;
using WorkerServiceTestBugRepro;
using Xunit;

namespace Tests
{
    public class WorkerTests
    {
        [Fact]
        public async Task ShouldSwallowExceptions_AndStopApplication()
        {           
            var sqsMessageQueue = new Mock<IQueueReader>();
            sqsMessageQueue.Setup(x => x.HasMessage()).ThrowsAsync(new Exception(""));

            // this version works due to the delay before throwing an exception
            //sqsMessageQueue.Setup(x => x.HasMessage()).ThrowsAsync(new Exception(""), TimeSpan.FromMilliseconds(100));

            var hostAppLifetime = new Mock<IHostApplicationLifetime>();

            var sut = new Worker(sqsMessageQueue.Object, hostAppLifetime.Object);

            await sut.StartAsync(default); 

            // we never reach here without the delay in the Moq

            await Task.Delay(250); // give it time to run

            await sut.StopAsync(default);

            hostAppLifetime.Verify(x => x.StopApplication(), Times.AtLeastOnce);
        }
    }
}
