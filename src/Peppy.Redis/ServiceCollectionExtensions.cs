﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Redis
{
    public static class ServiceCollectionExtensions
    {
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

        public static IServiceCollection AddPeppyRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PeppyRedisOptions>(configuration.GetSection("Redis"));
            services.AddSingleton<IRedisManager, RedisManager>();
            return services;
        }
    }
}