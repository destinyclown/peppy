using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Peppy.Quartz
{
    public class QuartzJobManager : IQuartzJobManager
    {
        private static IScheduler _scheduler;

        /// <summary>
        /// Start scheduler
        /// </summary>
        /// <returns></returns>
        public static async Task Start()
        {
            //开始调度器
            await _scheduler.Start();
        }

        /// <summary>
        /// Stop scheduler
        /// </summary>
        public void Stop()
        {
            _scheduler.Shutdown();
        }

        public static void CreateScheduler()
        {
            //创建一个调度器
            _scheduler = new StdSchedulerFactory().GetScheduler().Result;
        }

        /// <summary>
        /// ScheduleJob
        /// </summary>
        /// <param name="jobDetail"></param>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public static async Task ScheduleJob(IJobDetail jobDetail,
            ITrigger trigger)
        {
            await _scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}