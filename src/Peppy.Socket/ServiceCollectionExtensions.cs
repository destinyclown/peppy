using System;
using System.Collections.Generic;
using System.Text;
using Peppy.Socket.Manager;
using Microsoft.Extensions.DependencyInjection;

namespace Peppy.Socket
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSocket(this IServiceCollection services,
            Action<SocketOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            services.Configure(options);
            services.AddSingleton<ISocketManager, SocketManager>();
            return services;
        }
    }
}