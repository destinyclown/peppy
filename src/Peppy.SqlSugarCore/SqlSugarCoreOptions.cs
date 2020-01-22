using SqlSugar;
using System;

namespace Peppy.SqlSugarCore
{
    public class SqlSugarCoreOptions
    {
        /// <summary>
        /// Gets or sets the database's connection string that will be used to store database entities.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// SqlSugar Db Type
        /// </summary>
        public DbType DbType { get; set; }

        internal string Version { get; set; }
    }
}