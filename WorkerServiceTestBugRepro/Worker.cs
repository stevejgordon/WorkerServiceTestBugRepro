using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace WorkerServiceTestBugRepro
{

    public class Worker : BackgroundService
    {
        private readonly IQueueReader _queueReader;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public Worker(IQueueReader queueReader, IHostApplicationLifetime hostApplicationLifetime)
        {
            _queueReader = queueReader;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _queueReader.HasMessage();
                }
                catch (OperationCanceledException)
                {
                    // swallow as likely shutting down
                }
                catch (Exception)
                {
                    // unhandled in the main code so kill the app

                    _hostApplicationLifetime.StopApplication();
                }
            }
        }
    }
}
