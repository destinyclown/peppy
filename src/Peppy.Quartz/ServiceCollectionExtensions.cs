using Microsoft.Extensions.DependencyInjection;
using System;
using Quartz;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Peppy.Quartz.Manager;
using Quartz.Impl;

namespace Peppy.Quartz
{
    /// <summary>
    /// Extensions method
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 初始化任务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddQuartzJob(this IServiceCollection services)

        {
            services.AddSingleton<IQuartzJobManager, QuartzJobManager>();
            return services;
        }
    }
}