using Peppy.Dependency;
using Peppy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Peppy.Domain.Repositories
{
    /// <summary>
    /// A shortcut of <see cref="IRepository{TEntity,TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface IRepository<TEntity> : IRepository<TEntity, int>
        where TEntity : class, IEntity<int>
    {
    }

    /// <summary>
    /// This interface must be implemented by all repositories to identify them by convention.
    /// </summary>
    public interface IRepository<TEntity, TPrimaryKey> : ITransientDependency
        where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Qurey

        /// <summary>
        /// Used to get a IQueryable that is used to retrieve entities from entire table.
        /// </summary>
        /// <returns>IQueryable to be used to select entities from database</returns>
        IQueryable<TEntity> Query();

        /// <summary>
        /// Used to get a IQueryable that is used to retrieve entities from entire table.
        /// </summary>
        /// <param name="propertySelectors"></param>
        /// <returns>IQueryable to be used to select entities from database</returns>
        IQueryable<TEntity> QueryIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

        /// <summary>
        /// Used to query a array of entities from datatable
        /// </summary>
        /// <returns>Array of entities</returns>
        Task<TEntity[]> QueryArrayAsync();

        /// <summary>
        /// Used to query a array of entities from datatable by predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Array of entities</returns>
        //Task<TEntity[]> QueryArrayAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Used to query a list of entities from datatable
        /// </summary>
        /// <returns>List of entities</returns>
        Task<List<TEntity>> QueryListAsync();

        /// <summary>
        /// Used to query a list of entities from datatable by predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>List of entities</returns>
        //Task<List<TEntity>> QueryListAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Used to query a single entity from datatable by predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Entity</returns>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Used to query a first Or default entity by it's primary key id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entity</returns>
        Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id);

        /// <summary>
        /// Gets an entity with given given predicate or null if not found.
        /// </summary>
        /// <param name="predicate">Predicate to filter entities</param>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion Qurey

        #region Insert

        /// <summary>
        /// Insert a new entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="submit"></param>
        /// <returns>Inserted entity</returns>
        Task<TEntity> InsertAsync(TEntity entity, bool submit = true);

        /// <summary>
        /// Insert a new entity And return it's primary key id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Id of the entity</returns>
        Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);

        #endregion Insert

        #region Update

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="submit"></param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TEntity entity, bool submit = true);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction">Action that can be used to change values of the entity</param>
        /// <param name="submit"></param>
        /// <returns>Updated entity</returns>
        Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction, bool submit = true);

        #endregion Update

        #region Delete

        /// <summary>
        /// Deletes an entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="submit"></param>
        /// <returns>Entity to be deleted</returns>
        Task DeleteAsync(TEntity entity, bool submit = true);

        /// <summary>
        /// Deletes an entity by it's primary key id
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <param name="submit"></param>
        /// <returns>Primary key of the entity</returns>
        Task DeleteAsync(TPrimaryKey id, bool submit = true);

        #endregion Delete
    }
}