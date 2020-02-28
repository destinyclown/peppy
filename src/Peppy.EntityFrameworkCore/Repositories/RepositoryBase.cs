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
    public abstract class RepositoryBase<TDbContext, TEntity, TPrimaryKey> : IScopedDependency
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
        public virtual DbSet<TEntity> Table => Context.Set<TEntity>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="dbContextProvider"></param>
        protected RepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
        {
            Context = dbContextProvider.GetDbContext();
        }

        #region Qurey

        /// <summary>
        /// Used to get a IQueryable that is used to retrieve entities from entire table.
        /// </summary>
        /// <returns>IQueryable to be used to select entities from database</returns>
        protected virtual IQueryable<TEntity> Query()
        {
            if (!(Context.Set<TEntity>() is IQueryable<TEntity> query))
                throw new Exception($"{typeof(TEntity)} TEntity cannot be empty！");
            return query;
        }

        /// <summary>
        /// Used to get a IQueryable that is used to retrieve entities from entire table.
        /// </summary>
        /// <param name="propertySelectors"></param>
        /// <returns>IQueryable to be used to select entities from database</returns>
        protected virtual IQueryable<TEntity> QueryIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            if (propertySelectors == null || !propertySelectors.Any())
            {
                return Query();
            }

            var query = Query();

            return propertySelectors.Aggregate(query, (current, propertySelector) => current.Include(propertySelector));
        }

        /// <summary>
        /// Used to query a array of entities from data table
        /// </summary>
        /// <returns>Array of entities</returns>
        protected virtual async Task<TEntity[]> QueryArrayAsync()
        {
            return await Query().ToArrayAsync();
        }

        /// <summary>
        /// Used to query a array of entities from data table by predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Array of entities</returns>
        protected virtual async Task<TEntity[]> QueryArrayAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Query().Where(predicate).ToArrayAsync();
        }

        /// <summary>
        /// Used to query a list of entities from data table
        /// </summary>
        /// <returns>List of entities</returns>
        protected virtual async Task<List<TEntity>> QueryListAsync()
        {
            return await Query().ToListAsync();
        }

        /// <summary>
        /// Used to query a list of entities from data table by predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>List of entities</returns>
        protected virtual async Task<List<TEntity>> QueryListAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return await Query().Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Used to query a single entity from datatable by predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Entity</returns>
        protected virtual async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Query().SingleAsync(predicate);
        }

        /// <summary>
        /// Used to query a first Or default entity by it's primary key id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entity</returns>
        protected virtual async Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return await Query().FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        /// <summary>
        /// Gets an entity with given given predicate or null if not found.
        /// </summary>
        /// <param name="predicate">Predicate to filter entities</param>
        protected virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Query().FirstOrDefaultAsync(predicate);
        }

        #endregion Qurey

        #region Insert

        /// <summary>
        /// Insert a new entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Inserted entity</returns>
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            var result = await Table.AddAsync(entity);
            return result?.Entity;
        }

        /// <summary>
        /// Insert a new entity And return it's primary key id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Id of the entity</returns>
        public virtual async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            entity = await InsertAsync(entity);

            if (entity.IsTransient())
            {
                await Context.SaveChangesAsync();
            }

            return entity.Id;
        }

        #endregion Insert

        #region Update

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return await Task.FromResult(entity);
        }

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction">Action that can be used to change values of the entity</param>
        /// <returns>Updated entity</returns>
        public virtual async Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction)
        {
            var entity = await FirstOrDefaultAsync(id);
            await updateAction(entity);
            return entity;
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// Deletes an entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity to be deleted</returns>
        public virtual async Task DeleteAsync(TEntity entity)
        {
            await Task.FromResult(Table.Remove(entity));
        }

        /// <summary>
        /// Deletes an entity by it's primary key id
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns>Primary key of the entity</returns>
        public virtual async Task DeleteAsync(TPrimaryKey id)
        {
            var entity = Table.Local.FirstOrDefault(ent => EqualityComparer<TPrimaryKey>.Default.Equals(ent.Id, id));
            if (entity == null)
            {
                entity = await Query().FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
                if (entity == null)
                {
                    return;
                }
            }

            await DeleteAsync(entity);
        }

        #endregion Delete

        #region Expression

        /// <summary>
        ///
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void AttachIfNot(TEntity entity)
        {
            if (!Table.Local.Contains(entity))
            {
                Table.Attach(entity);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var leftExpression = Expression.PropertyOrField(lambdaParam, "Id");

            var idValue = Convert.ChangeType(id, typeof(TPrimaryKey));

            Expression<Func<object>> closure = () => idValue;
            var rightExpression = Expression.Convert(closure.Body, leftExpression.Type);

            var lambdaBody = Expression.Equal(leftExpression, rightExpression);

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

        #endregion Expression
    }
}