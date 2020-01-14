using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Peppy.Redis;
using Peppy.Redis.Manager;
using System;

namespace Peppy.Extensions
{
    /// <summary>
    /// Extensions method
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Redis service registered
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options">Redis config Options</param>
        /// <returns></returns>
        public static IServiceCollection AddPeppyRedis(this IServiceCollection services, Action<PeppyRedisOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            services.Configure(options);
            services.AddSingleton<IRedisManager, RedisManager>();
            return services;
        }

        /// <summary>
        /// Redis service registered
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddPeppyRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PeppyRedisOptions>(configuration.GetSection("Redis"));
            services.AddSingleton<IRedisManager, RedisManager>();
            return services;
        }
    }
}