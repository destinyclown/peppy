using Microsoft.EntityFrameworkCore;
using Peppy.Domain.UnitOfWork;
using Peppy.EntityFrameworkCore;
using Peppy.EntityFrameworkCore.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions method
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddEntityFrameworkCore<TContext>(this IServiceCollection services, string connectionString)
            where TContext : DbContext
        {
            services.AddEntityFrameworkCore<TContext>(otp => { otp.ConnectionString = connectionString; });
            return services;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddEntityFrameworkCore<TContext>(this IServiceCollection services, Action<EFCoreOptions> options)
            where TContext : DbContext
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            services.AddDbContext<TContext>();
            services.Configure(options);
            services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager<TContext>>();
            services.AddScoped<IUnitOfWorkCompleteHandle, UnitOfWorkCompleteHandle<TContext>>();
            return services;
        }
    }
}