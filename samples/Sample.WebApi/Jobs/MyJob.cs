using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace Sample.WebApi.Jobs
{
    /// <summary>
    /// 创建IJob的实现类，并实现Excute方法。
    /// </summary>
    public class MyJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            if (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Run(() =>
                {
                    Console.WriteLine($"{context.JobDetail.Key}：{DateTime.Now:yyyy-MM-dd HH-mm-ss}");
                });
            }
        }
    }
}