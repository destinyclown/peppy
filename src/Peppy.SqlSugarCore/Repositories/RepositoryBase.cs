using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Peppy.Dependency;
using Peppy.Domain.Entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Peppy.SqlSugarCore.Repositories
{
    /// <summary>
    /// Implements IRepository for Entity Framework Core
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IScopedDependency
            where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        private readonly SqlSugarCoreOptions _options;

        public RepositoryBase(IDbContextProvider dbContextProvider)
        {
            _options = dbContextProvider.GetOptions();
        }

        #region Qurey

        /// <summary>
        /// Create SqlSugarClient
        /// </summary>
        /// <returns></returns>
        private SqlSugarClient Query()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = _options.ConnectionString,
                DbType = _options.DbType,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });

            //Print sql
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                if (db.TempItems == null) db.TempItems = new Dictionary<string, object>();
                db.TempItems.Add("logTime", DateTime.Now);
            };
            db.Aop.OnLogExecuted = (sql, pars) =>
            {
                var startingTime = (DateTime)db.TempItems["logTime"];
                db.TempItems.Remove("logtime");
                var completedTime = DateTime.Now;
                var tiemOut = completedTime.Subtract(startingTime);
                if (pars.Any())
                {
                    SqlLoggerUtil.LogSql(sql, pars, tiemOut);
                }
                else
                {
                    SqlLoggerUtil.LogSql(sql, tiemOut);
                }
            };
            return db;
        }

        /// <summary>
        /// Used to query a list of entities from datatable
        /// </summary>
        /// <returns>List of entities</returns>
        protected virtual async Task<List<TEntity>> QueryListAsync()
        {
            return await Query().Queryable<TEntity>().ToListAsync();
        }

        /// <summary>
        /// Used to query a list of entities from datatable by predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>List of entities</returns>
        protected virtual async Task<List<TEntity>> QueryListAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return await Query().Queryable<TEntity>().Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Used to query a single entity from datatable by predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Entity</returns>
        protected virtual async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Query().Queryable<TEntity>().SingleAsync(predicate);
        }

        /// <summary>
        /// Used to query a first Or default entity by it's primary key id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entity</returns>
        protected virtual async Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return await Query().Queryable<TEntity>().FirstAsync(CreateEqualityExpressionForId(id));
        }

        /// <summary>
        /// Gets an entity with given given predicate or null if not found.
        /// </summary>
        /// <param name="predicate">Predicate to filter entities</param>
        protected virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Query().Queryable<TEntity>().FirstAsync(predicate);
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
            var result = await Query().Insertable(entity).ExecuteCommandAsync();
            return entity;
        }

        /// <summary>
        /// Insert a new entity And return it's primary key id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Id of the entity</returns>
        public virtual async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            await Query().Insertable(entity).ExecuteReturnIdentityAsync();
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
            var result = await Query().Updateable(entity).ExecuteCommandAsync();
            return entity;
        }

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction">Action that can be used to change values of the entity</param>
        /// <returns>Updated entity</returns>
        public virtual async Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction)
        {
            var entity = await Query().Queryable<TEntity>().FirstAsync(x => x.Id.Equals(id));
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
            await Query().Deleteable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// Deletes an entity by it's primary key id
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns>Primary key of the entity</returns>
        public virtual async Task DeleteAsync(TPrimaryKey id)
        {
            var entity = await Query().Queryable<TEntity>().FirstAsync(ent => EqualityComparer<TPrimaryKey>.Default.Equals(ent.Id, id));
            if (entity == null)
            {
                entity = await Query().Queryable<TEntity>().FirstAsync(CreateEqualityExpressionForId(id));
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