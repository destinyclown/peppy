using Microsoft.EntityFrameworkCore;
using Peppy.Domain.UnitOfWork;
using Peppy.EntityFrameworkCore;
using Peppy.EntityFrameworkCore.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

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
        /// <param name="isUseLogger"></param>
        /// <returns></returns>
        public static IServiceCollection AddEntityFrameworkCore<TContext>(this IServiceCollection services, string connectionString, bool isUseLogger = true)
            where TContext : EFCroeDbContext
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
        /// <param name="isUseLogger"></param>
        /// <returns></returns>
        public static IServiceCollection AddEntityFrameworkCore<TContext>(this IServiceCollection services, Action<EFCoreOptions> options, bool isUseLogger = true)
            where TContext : EFCroeDbContext
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            services.AddDbContext<TContext>(optionsAction =>
            {
                //使用ef core mysql 连接
                if (!isUseLogger) return;
                var loggerFactory = new LoggerFactory();
                loggerFactory.AddProvider(new EFLoggerProvider());
                optionsAction.UseLoggerFactory(loggerFactory);
            });
            services.AddTransient<IUnitOfWorkManager, UnitOfWorkManager<TContext>>();
            services.AddTransient<IUnitOfWorkCompleteHandle, UnitOfWorkCompleteHandle<TContext>>();
            services.AddTransient<IDbContextProvider<TContext>, DbContextProvider<TContext>>();
            return services;
        }
    }
}