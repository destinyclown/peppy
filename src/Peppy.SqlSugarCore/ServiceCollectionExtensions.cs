using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.SqlSugarCore
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
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugarCore(this IServiceCollection services, string connectionString)

        {
            services.AddSqlSugarCore(otp => { otp.ConnectionString = connectionString; });
            return services;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugarCore(this IServiceCollection services, Action<SqlSugarCoreOptions> options)

        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            services.Configure(options);
            services.AddScoped<IDbContextProvider, DbContextProvider>();
            return services;
        }
    }
}