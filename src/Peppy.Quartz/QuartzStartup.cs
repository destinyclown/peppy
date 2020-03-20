using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Linq;

namespace Peppy.Quartz
{
    using IOCContainer = IServiceProvider;

    // Quartz.Net启动后注册job和trigger
    public class QuartzStartup
    {
        public IScheduler _scheduler { get; set; }
        private readonly ILogger _logger;
        private readonly IJobFactory iocJobfactory;

        public QuartzStartup(IOCContainer IocContainer, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<QuartzStartup>();
            iocJobfactory = new IOCJobFactory(IocContainer);
            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
            _scheduler.JobFactory = iocJobfactory;
        }

        public void Start()
        {
            _logger.LogInformation("Schedule job load as application start.");
            _scheduler.Start().Wait();
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IJob))))
                .ToArray();
            var jobCount = 0;
            foreach (var type in types)
            {
                foreach (QuartzJobAttribute quartzJob in type.GetCustomAttributes(typeof(QuartzJobAttribute), true))
                {
                    var jobDetail = JobBuilder.Create(type)
                        .WithIdentity(quartzJob.Name, quartzJob.Group)
                        .Build();

                    var trigger = TriggerBuilder.Create()
                        .WithIdentity(quartzJob.Name, quartzJob.Group)
                        // Seconds,Minutes,Hours，Day-of-Month，Month，Day-of-Week，Year（optional field）
                        .If(quartzJob.Action != null, x => x.WithSimpleSchedule(quartzJob.Action))
                        .If(!string.IsNullOrEmpty(quartzJob.Cron), x => x.WithCronSchedule(quartzJob.Cron))
                        .StartNow();

                    foreach (QuartzDataAttribute quartzData in type.GetCustomAttributes(typeof(QuartzDataAttribute), true))
                    {
                        trigger.AddJobData(quartzData.Key, quartzData.Value, quartzData.ValueType);
                    }
                    jobCount++;
                    _scheduler.ScheduleJob(jobDetail, trigger.Build()).Wait();
                    _scheduler.TriggerJob(new JobKey(quartzJob.Name, quartzJob.Group));
                }
            }
            _logger.LogInformation($"{jobCount} quartz jobs were successfully initialized. Schedule job load end.");
        }

        public void Stop()
        {
            if (_scheduler == null)
            {
                return;
            }
            if (_scheduler.Shutdown(waitForJobsToComplete: true).Wait(30000))
                _scheduler = null;
            else
            {
            }
            _logger.LogCritical("Schedule job upload as application stopped.");
        }
    }
}