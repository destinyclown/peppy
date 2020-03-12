using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Peppy.Quartz
{
    public abstract class QuartzJob : IJob
    {
        private readonly ILogger<QuartzJob> _logger;
        protected IJobExecutionContext _context;

        public QuartzJob()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            _logger = loggerFactory.CreateLogger<QuartzJob>();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _context = context;
            if (!context.CancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"[{DateTimeOffset.Now}] {this.GetType().Name} start to perform");
                var watch = Stopwatch.StartNew();
                watch.Restart();
                await ExecuteAsync();
                _logger.LogInformation($"[{DateTimeOffset.Now}] {this.GetType().Name} completes, elapsed time: {(watch.ElapsedMilliseconds / 1000D).ToString("0.000")}s");
            }
        }

        protected abstract Task ExecuteAsync();
    }
}