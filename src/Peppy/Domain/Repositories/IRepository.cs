using Peppy.Dependency;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Peppy.Domain.Repositories
{
    /// <summary>
    /// This interface must be implemented by all repositories to identify them by convention.
    /// </summary>
    public interface IRepository<TEntity, TPrimaryKey> : IScopedDependency
    {
        /// <summary>
        /// get a single entity
        /// </summary>
        /// <returns></returns>
        TEntity Single();

        /// <summary>
        /// get a single entity
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity Single(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// get a single entity
        /// </summary>
        /// <returns></returns>
        Task<TEntity> SingleAsync();

        /// <summary>
        /// get a single entity
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);
    }
}