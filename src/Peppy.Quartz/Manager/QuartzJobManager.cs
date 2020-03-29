using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Quartz.Impl.Matchers;

namespace Peppy.Quartz.Manager
{
    public class QuartzJobManager : IQuartzJobManager
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public QuartzJobManager(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        public async Task DeleteJobAsync<TJob>(string schedulerName = "DefaultQuartzScheduler") where TJob : IJob
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TJob"></typeparam>
        /// <param name="keyValues"></param>
        /// <param name="schedulerName"></param>
        /// <returns></returns>
        public async Task RunJobAsync<TJob>(JobDataMap keyValues = null, string schedulerName = "DefaultQuartzScheduler") where TJob : IJob
        {
            foreach (QuartzJobAttribute quartzJob in typeof(TJob).GetCustomAttributes(typeof(QuartzJobAttribute), true))
            {
                var scheduler = await _schedulerFactory.GetScheduler(schedulerName);
                var jobKey = new JobKey(quartzJob.Name, quartzJob.Group);
                scheduler.ListenerManager.AddJobListener(new JobListener(), GroupMatcher<JobKey>.AnyGroup());
                if (await scheduler.CheckExists(jobKey))
                {
                    await scheduler.ResumeJob(jobKey);
                    continue;
                }
                var jobDetail = JobBuilder.Create<TJob>()
                    .WithIdentity(quartzJob.Name, quartzJob.Group)
                    .Build();

                var trigger = TriggerBuilder.Create()
                    .WithIdentity(quartzJob.Name, quartzJob.Group)
                    .StartNow();

                if (!string.IsNullOrEmpty(quartzJob.Cron))
                {
                    trigger.WithCronSchedule(quartzJob.Cron);
                }
                else
                {
                    trigger.WithSimpleSchedule(x => x.WithRepeatCount(0));
                }
                
                await scheduler.ScheduleJob(jobDetail, trigger.Build());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TJob"></typeparam>
        /// <param name="schedulerName"></param>
        /// <returns></returns>
        public Task ShopJobAsync<TJob>(string schedulerName = "DefaultQuartzScheduler") where TJob : IJob
        {
            throw new NotImplementedException();
        }
    }
}
