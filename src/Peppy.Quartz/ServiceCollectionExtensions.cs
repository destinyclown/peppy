using Microsoft.Extensions.DependencyInjection;
using System;
using Quartz;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
        /// <param name="job"></param>
        /// <returns></returns>
        public static IServiceCollection AddQuartzJob(this IServiceCollection services)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IJob))))
                .ToArray();
            foreach (var type in types)
            {
                foreach (QuartzJobAttribute quartzJob in type.GetCustomAttributes(typeof(QuartzJobAttribute), true))
                {
                    services.AddTransient(type);
                }
            }
            services.AddSingleton<QuartzStartup>();
            return services;
        }
    }
}