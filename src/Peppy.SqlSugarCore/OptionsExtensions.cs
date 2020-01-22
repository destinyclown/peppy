using Peppy;
using Peppy.SqlSugarCore;
using SqlSugar;
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
        /// <param name="options"></param>
        /// <param name="connectionString"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static PeppyOptions UseSqlSugarCore(this PeppyOptions options, string connectionString, DbType dbType)
        {
            return options.UseSqlSugarCore(opt => { opt.ConnectionString = connectionString; opt.DbType = dbType; });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static PeppyOptions UseSqlSugarCore(this PeppyOptions options, Action<SqlSugarCoreOptions> configure)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.RegisterExtension(new SqlSugarCoreOptionsExtension(configure));

            return options;
        }
    }
}