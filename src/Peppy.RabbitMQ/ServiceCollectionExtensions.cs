using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Peppy.Core.Amqp.Internal;
using Peppy.RabbitMQ;
using Peppy.RabbitMQ.Manager;
using System;

namespace Peppy.RabbitMQ
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPeppyRabbitMQ(this IServiceCollection services, Action<PeppyRabbitMQOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            services.Configure(options);
            services.AddSingleton<IRabbitMQManager, RabbitMQManager>();
            services.AddSingleton<ISubscribeInvokerFactory, ConsumerInvokerFactory>();
            services.AddSingleton<ClientRegister>();
            return services;
        }

        public static IServiceCollection AddPeppyRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PeppyRabbitMQOptions>(configuration.GetSection("Redis"));
            services.AddSingleton<IRabbitMQManager, RabbitMQManager>();
            services.TryAddSingleton<ClientRegister>();
            return services;
        }
    }
}