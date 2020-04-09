using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quartz;

namespace Peppy.Quartz
{
    public class JobListener : IJobListener
    {
        public string Name { get; } = nameof(JobListener);

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            //Job即将执行
            return Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"[{DateTimeOffset.Now}] {context.JobDetail.Key} 即将执行");
            });
        }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"[{DateTimeOffset.Now}] {context.JobDetail.Key} 被否决执行");
            });
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            //Job执行完成
            return Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"[{DateTimeOffset.Now}] {context.JobDetail.Key} 执行完成");
            });
        }
    }
}
