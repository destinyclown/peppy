using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Peppy.Domain.Entities;
using Peppy.Domain.Repositories;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Extensions;

namespace Peppy.EntityFrameworkCore.Plus
{
    /// <summary>
    /// Entity Framework Core Plus Extensions
    /// </summary>
    public static class EntityFrameworkCorePlusExtensions
    {
        //#region BatchDelete

        ///// <summary>
        ///// Deletes all matching entities permanently for given predicate
        ///// </summary>
        ///// <typeparam name="TEntity">Entity type</typeparam>
        ///// <typeparam name="TPrimaryKey">Primary key type</typeparam>
        ///// <param name="repository">Repository</param>
        ///// <param name="predicate">Predicate to filter entities</param>
        ///// <param name="batchDeleteBuilder">The batch delete builder to change default configuration.</param>
        ///// <returns></returns>
        //public static async Task<int> BatchDeleteAsync<TEntity, TPrimaryKey>(
        //    [NotNull] this IRepository<TEntity, TPrimaryKey> repository,
        //    [NotNull] Expression<Func<TEntity, bool>> predicate,
        //    Action<BatchDelete> batchDeleteBuilder = null)
        //    where TEntity : Entity<TPrimaryKey>
        //{
        //    var query = repository.Query().IgnoreQueryFilters();

        //    query = query.Where(predicate);

        //    return await query.DeleteFromQueryAsync(batchDeleteBuilder);
        //}

        ///// <summary>
        ///// Deletes all matching entities permanently for given primaryKeys
        ///// </summary>
        ///// <typeparam name="TEntity">Entity type</typeparam>
        ///// <typeparam name="TPrimaryKey">Primary key type</typeparam>
        ///// <param name="repository">Repository</param>
        ///// <param name="primaryKeys">Primary keys</param>
        ///// <param name="batchDeleteBuilder">The batch delete builder to change default configuration.</param>
        ///// <returns></returns>
        //public static async Task<int> BatchDeleteAsync<TEntity, TPrimaryKey>(
        //    [NotNull] this IRepository<TEntity, TPrimaryKey> repository,
        //    [NotNull] TPrimaryKey[] primaryKeys,
        //    Action<BatchDelete> batchDeleteBuilder = null)
        //    where TEntity : Entity<TPrimaryKey>
        //{
        //    return await repository.BatchDeleteAsync(x => primaryKeys.Contains(x.Id), batchDeleteBuilder);
        //}

        ///// <summary>
        ///// Deletes all matching entities permanently for given predicate
        ///// </summary>
        ///// <typeparam name="TEntity">Entity type</typeparam>
        ///// <param name="repository">Repository</param>
        ///// <param name="predicate">Predicate to filter entities</param>
        ///// <param name="batchDeleteBuilder">The batch delete builder to change default configuration.</param>
        ///// <returns></returns>
        //public static async Task<int> BatchDeleteAsync<TEntity>(
        //    [NotNull] this IRepository<TEntity> repository,
        //    [NotNull]Expression<Func<TEntity, bool>> predicate,
        //    Action<BatchDelete> batchDeleteBuilder = null)
        //    where TEntity : Entity<int>
        //{
        //    return await repository.BatchDeleteAsync<TEntity, int>(predicate, batchDeleteBuilder);
        //}

        //#endregion BatchDelete

        //#region BatchUpdate

        ///// <summary>
        ///// Updates all matching entities using given updateExpression for given predicate
        ///// </summary>
        ///// <typeparam name="TEntity">Entity type</typeparam>
        ///// <typeparam name="TPrimaryKey">Primary key type</typeparam>
        ///// <param name="repository">Repository</param>
        ///// /// <param name="updateExpression">Update expression</param>
        ///// <param name="predicate">Predicate to filter entities</param>
        ///// <param name="batchUpdateBuilder">The batch delete builder to change default configuration.</param>
        ///// <returns></returns>
        //public static async Task<int> BatchUpdateAsync<TEntity, TPrimaryKey>(
        //    [NotNull]this IRepository<TEntity, TPrimaryKey> repository,
        //    [NotNull]Expression<Func<TEntity, TEntity>> updateExpression,
        //    [NotNull]Expression<Func<TEntity, bool>> predicate,
        //    Action<BatchUpdate> batchUpdateBuilder = null)
        //    where TEntity : Entity<TPrimaryKey>
        //{
        //    var query = repository.Query().IgnoreQueryFilters();

        //    query = query.Where(predicate);

        //    return await query.UpdateFromQueryAsync(updateExpression, batchUpdateBuilder);
        //}

        ///// <summary>
        ///// Updates all matching entities using given updateExpression for given predicate
        ///// </summary>
        ///// <typeparam name="TEntity">Entity type</typeparam>
        ///// <param name="repository">Repository</param>
        ///// /// <param name="updateExpression">Update expression</param>
        ///// <param name="predicate">Predicate to filter entities</param>
        ///// <param name="batchUpdateBuilder">The batch delete builder to change default configuration.</param>
        ///// <returns></returns>
        //public static async Task<int> BatchUpdateAsync<TEntity>(
        //    [NotNull]this IRepository<TEntity> repository, [NotNull]Expression<Func<TEntity, TEntity>> updateExpression,
        //    [NotNull]Expression<Func<TEntity, bool>> predicate,
        //    Action<BatchUpdate> batchUpdateBuilder = null)
        //    where TEntity : Entity<int>
        //{
        //    return await repository.BatchUpdateAsync<TEntity, int>(updateExpression, predicate, batchUpdateBuilder);
        //}

        //#endregion BatchUpdate
    }
}