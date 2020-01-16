using Microsoft.EntityFrameworkCore;
using Peppy.Dependency;
using Peppy.Domain.Entities;
using Peppy.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Peppy.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// Implements IRepository for Entity Framework Core
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class RepositoryBase<TDbContext, TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
            where TDbContext : DbContext
            where TEntity : class, IEntity<TPrimaryKey>
    {
        /// <summary>
        ///
        /// </summary>
        public virtual TDbContext Context { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dbContextProvider"></param>
        public RepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
        {
            Context = dbContextProvider.GetDbContext();
        }

        /// <summary>
        /// get a single entity
        /// </summary>
        /// <returns></returns>
        public virtual TEntity Single()
        {
            return Query().FirstOrDefault();
        }

        /// <summary>
        /// get a single entity
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return Query().FirstOrDefault(predicate);
        }

        /// <summary>
        /// get a single entity
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TEntity> SingleAsync()
        {
            return await Query().FirstOrDefaultAsync();
        }

        /// <summary>
        /// get a single entity
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Query().FirstOrDefaultAsync(predicate);
        }

        private IQueryable<TEntity> Query()
        {
            var query = Context.Set<TEntity>() as IQueryable<TEntity>;
            if (query == null)
                throw new Exception($"{typeof(TEntity)} TEntity cannot be empty！");
            return query;
        }
    }
}