using Microsoft.EntityFrameworkCore;
using Peppy.Domain.UnitOfWork;
using Peppy.EntityFrameworkCore;
using Peppy.EntityFrameworkCore.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Peppy.EntityFrameworkCore.Repositories;

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
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="isUseLogger"></param>
        /// <returns></returns>
        public static IServiceCollection AddEntityFrameworkCore<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> options, bool isUseLogger = true)
            where TDbContext : EFCroeDbContext
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            //var efOptions = new EFCoreOptions<TDbContext>();
            //options(efOptions);
            //if (efOptions.DbContextOptions == null)
            //{
            //    throw new ArgumentNullException(nameof(efOptions.DbContextOptions));
            //}
            services.AddDbContext<TDbContext>(options, ServiceLifetime.Singleton);
            //services.AddDbContext<TDbContext>(optionsAction =>
            //{
            //    optionsAction = efOptions.DbContextOptions;
            //    if (!isUseLogger) return;
            //    var loggerFactory = new LoggerFactory();
            //    loggerFactory.AddProvider(new EFLoggerProvider());
            //    optionsAction.UseLoggerFactory(loggerFactory);
            //});
            services.AddSingleton<IUnitOfWorkManager, UnitOfWorkManager<TDbContext>>();
            //services.AddSingleton<IUnitOfWorkCompleteHandle, UnitOfWorkCompleteHandle<TDbContext>>();
            services.AddSingleton<IDbContextProvider<TDbContext>, DbContextProvider<TDbContext>>();
            services.AddSingleton(typeof(IRepositoryBase<,,>), typeof(RepositoryBase<,,>));
            services.AddSingleton(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            return services;
        }
    }
}