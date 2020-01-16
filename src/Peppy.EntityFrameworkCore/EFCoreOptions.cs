using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.EntityFrameworkCore
{
    /// <summary>
    /// Entity Framework Core Options
    /// </summary>
    public class EFCoreOptions
    {
        /// <summary>
        /// Gets or sets the database's connection string that will be used to store database entities.
        /// </summary>
        public string ConnectionString { get; set; }

        internal Type DbContextType { get; set; }

        internal string Version { get; set; }
    }
}