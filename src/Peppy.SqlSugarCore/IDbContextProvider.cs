using Peppy.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.SqlSugarCore
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IDbContextProvider : IScopedDependency
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        SqlSugarCoreOptions GetOptions();
    }
}