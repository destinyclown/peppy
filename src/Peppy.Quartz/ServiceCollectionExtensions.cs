using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Quartz;
using System.Linq;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;

namespace Peppy.Quartz
{
    /// <summary>
    /// Extensions method
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        /// <param name="job"></param>
        /// <returns></returns>
        public static IServiceCollection AddQuartzJob(this IServiceCollection services)
        {
            //初始化任务
            CreateQuartzJob();
            return services;
        }

        /// <summary>
        /// 初始化任务
        /// </summary>
        private static void CreateQuartzJob()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IJob))))
                .ToArray();
            QuartzJobManager.CreateScheduler();
            foreach (var type in types)
            {
                foreach (QuartzJobAttribute quartzJob in type.GetCustomAttributes(typeof(QuartzJobAttribute), true))
                {
                    //3、创建一个触发器
                    var trigger = TriggerBuilder.Create()
                        .If(quartzJob.Action != null, x => x.WithSimpleSchedule(quartzJob.Action))
                        .If(!string.IsNullOrEmpty(quartzJob.Cron), x => x.WithCronSchedule(quartzJob.Cron))
                        .StartNow();

                    foreach (QuartzDataAttribute quartzData in type.GetCustomAttributes(typeof(QuartzDataAttribute), true))
                    {
                        trigger.AddJobData(quartzData.Key, quartzData.Value, quartzData.ValueType);
                    }

                    //4、创建任务
                    var jobDetail = JobBuilder.Create(type)
                        .WithIdentity(quartzJob.Name, quartzJob.Group);

                    Task.Run(async () =>
                    {
                        await QuartzJobManager.ScheduleJob(jobDetail.Build(), trigger.Build());
                    });
                }
                Task.Run(async () =>
                {
                    await QuartzJobManager.Start();
                });
            }
        }

        /// <summary>
        /// add job data
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        private static void AddJobData(this TriggerBuilder trigger, string key, string value, Type type)
        {
            if (type.FullName == null) return;
            switch (type.FullName.ToLower())
            {
                case "int":
                    trigger.UsingJobData(key, int.Parse(value));
                    break;

                case "long":
                    trigger.UsingJobData(key, long.Parse(value));
                    break;

                case "float":
                    trigger.UsingJobData(key, float.Parse(value));
                    break;

                case "double":
                    trigger.UsingJobData(key, double.Parse(value));
                    break;

                case "decimal":
                    trigger.UsingJobData(key, decimal.Parse(value));
                    break;

                case "bool":
                    trigger.UsingJobData(key, bool.Parse(value));
                    break;

                default:
                    trigger.UsingJobData(key, value);
                    break;
            }
        }
    }
}