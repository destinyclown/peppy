using Microsoft.EntityFrameworkCore;
using Peppy;
using Peppy.EntityFrameworkCore;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Options extensions
    /// </summary>
    public static class OptionsExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="options"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static PeppyOptions UseEntityFrameworkCore<TContext>(this PeppyOptions options, string connectionString)
            where TContext : EFCroeDbContext
        {
            return options.UseEntityFrameworkCore<TContext>(opt => { opt.ConnectionString = connectionString; });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static PeppyOptions UseEntityFrameworkCore<TContext>(this PeppyOptions options)
            where TContext : EFCroeDbContext
        {
            return options.UseEntityFrameworkCore<TContext>(opt => { });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static PeppyOptions UseEntityFrameworkCore<TContext>(this PeppyOptions options, Action<EFCoreOptions> configure)
            where TContext : EFCroeDbContext
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.RegisterExtension(new EFCoreOptionsExtension<TContext>(configure));

            return options;
        }
    }
}