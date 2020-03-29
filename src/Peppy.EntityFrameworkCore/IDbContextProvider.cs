using Microsoft.EntityFrameworkCore;
using Peppy.Dependency;

namespace Peppy.EntityFrameworkCore
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IDbContextProvider<out TDbContext> : ITransientDependency
        where TDbContext : DbContext
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        TDbContext GetDbContext();
    }
}