using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Peppy.Quartz;
using Quartz;

namespace Sample.WebApi.Jobs
{
    /// <summary>
    /// 创建IJob的实现类，并实现Excute方法。
    /// </summary>
    [QuartzJob("job", "group", "0/5 * * * * ?")]
    public class MyJob : QuartzJob
    {
        private readonly ILogger _logger;

        public MyJob(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MyJob>();
        }

        //public async Task Execute(IJobExecutionContext context)
        //{
        //    if (!context.CancellationToken.IsCancellationRequested)
        //    {
        //        await Task.Run(() =>
        //        {
        //            Console.WriteLine($"{context.JobDetail.Key}：{DateTime.Now:yyyy-MM-dd HH-mm-ss}");
        //        });
        //    }
        //}

        /// <summary>
        /// 主逻辑
        /// </summary>
        /// <returns></returns>
        protected override async Task ExecuteAsync()
        {
            await Task.Delay(500);
            await Task.Run(() =>
            {
                _logger.LogInformation($"{_context.JobDetail.Key}：{DateTime.Now:yyyy-MM-dd HH-mm-ss}");
            });
        }
    }
}