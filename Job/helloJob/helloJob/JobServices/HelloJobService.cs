using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace helloJob.JobServices
{
    public class HelloJobService : IHostedService, IDisposable
    {
        private readonly ILogger _mLogger;
        private Timer _mTimer;

        public HelloJobService(ILogger<HelloJobService> logger) {
            _mLogger = logger;
        }

        public void Dispose()
        {
            _mTimer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _mLogger.LogInformation("work start");
            _mTimer = new Timer(DoWork, null,TimeSpan.Zero, TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }

        private void DoWork(object state) {
            _mLogger.LogInformation("[{0:yyyy-MM-dd HH:mm:ss}] Timed background job is working now.", DateTime.Now);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _mLogger.LogInformation("background Task stoped.");
            _mTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
