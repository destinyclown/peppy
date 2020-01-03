using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peppy.Core.Cache
{
    public interface ICacheManager
    {
        /// <summary>
        /// Add a new entity
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="key">Key</param>
        /// <param name="entity">Entity</param>
        /// <param name="cacheTime">Cache time</param>
        /// <returns></returns>
        bool Add<TEntity>(string key, TEntity entity, TimeSpan? cacheTime = null) where TEntity : class;

        /// <summary>
        /// Add a new array
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="key">Key</param>
        /// <param name="entities">An array</param>
        /// <param name="cacheTime">Cache time</param>
        /// <returns></returns>
        bool Add<TEntity>(string key, TEntity[] entities, TimeSpan? cacheTime = null) where TEntity : class;

        /// <summary>
        /// Add a new list
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="key">Key</param>
        /// <param name="entities">An list</param>
        /// <param name="cacheTime">Cache time</param>
        /// <returns></returns>
        bool Add<TEntity>(string key, IList<TEntity> entities, TimeSpan? cacheTime = null) where TEntity : class;

        /// <summary>
        /// Add a new entity
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="key">Key</param>
        /// <param name="entity">Entity</param>
        /// <param name="cacheTime">Cache time</param>
        /// <returns></returns>
        Task<bool> AddAsync<TEntity>(string key, TEntity entity, TimeSpan? cacheTime = null) where TEntity : class;

        /// <summary>
        /// Add a new array
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="key">Key</param>
        /// <param name="entities">An array</param>
        /// <param name="cacheTime">Cache time</param>
        /// <returns></returns>
        Task<bool> AddAsync<TEntity>(string key, TEntity[] entities, TimeSpan? cacheTime = null) where TEntity : class;

        /// <summary>
        /// Add a new list
        /// </summary>
        /// <typeparam name="TEntity">entity</typeparam>
        /// <param name="key">key</param>
        /// <param name="entities">a list</param>
        /// <param name="cacheTime">cache time</param>
        /// <returns></returns>
        Task<bool> AddAsync<TEntity>(string key, IList<TEntity> entities, TimeSpan? cacheTime = null) where TEntity : class;

        /// <summary>
        /// Get list tatal
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        long Count(string key);

        /// <summary>
        /// Get list tatal
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        Task<long> CountAsync(string key);

        /// <summary>
        /// Set cache time
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="cacheTime">Cache time</param>
        /// <returns></returns>
        bool Expire(string key, TimeSpan cacheTime);

        /// <summary>
        /// Set cache time
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="cacheTime">Cache time</param>
        /// <returns></returns>
        /// <returns></returns>
        Task<bool> ExpireAsync(string key, TimeSpan cacheTime);

        /// <summary>
        /// Remove by key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        bool Remove(string key);

        /// <summary>
        /// Remove by key array
        /// </summary>
        /// <param name="keys">Key array</param>
        /// <returns></returns>
        bool Remove(string[] keys);

        /// <summary>
        /// Remove by key list
        /// </summary>
        /// <param name="keys">Key list</param>
        /// <returns></returns>
        bool Remove(IList<string> keys);

        /// <summary>
        /// Remove by key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// Remove by key array
        /// </summary>
        /// <param name="keys">Key array</param>
        /// <returns></returns>
        Task<bool> RemoveAsync(string[] keys);

        /// <summary>
        /// Remove by key list
        /// </summary>
        /// <param name="keys">Key list</param>
        /// <returns></returns>
        Task<bool> RemoveAsync(IList<string> keys);

        /// <summary>
        /// Updates an entity by key
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="key">Key</param>
        /// <param name="entity">Entity</param>
        /// <param name="cacheTime">Cache time</param>
        /// <returns></returns>
        bool Update<TEntity>(string key, TEntity entity, TimeSpan? cacheTime = null) where TEntity : class;

        /// <summary>
        /// Updates an entity by key
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="key">Key</param>
        /// <param name="entities">An array</param>
        /// <param name="cacheTime">Cache time</param>
        /// <returns></returns>
        bool Update<TEntity>(string key, TEntity[] entities, TimeSpan? cacheTime = null) where TEntity : class;

        /// <summary>
        /// Updates an entity by key
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="key">Key</param>
        /// <param name="entities">An list</param>
        /// <param name="cacheTime">Cache time</param>
        /// <returns></returns>
        bool Update<TEntity>(string key, IList<TEntity> entities, TimeSpan? cacheTime = null) where TEntity : class;

        /// <summary>
        /// Updates an entity by key
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="key">Key</param>
        /// <param name="entity">Entity</param>
        /// <param name="cacheTime">Cache time</param>
        /// <returns></returns>
        Task<bool> UpdateAsync<TEntity>(string key, TEntity entity, TimeSpan? cacheTime = null) where TEntity : class;

        /// <summary>
        /// Updates an entity by key
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="key">Key</param>
        /// <param name="entities">An array</param>
        /// <param name="cacheTime">Cache time</param>
        /// <returns></returns>
        Task<bool> UpdateAsync<TEntity>(string key, TEntity[] entities, TimeSpan? cacheTime = null) where TEntity : class;

        /// <summary>
        /// Updates an entity by key
        /// </summary>
        /// <typeparam name="TEntity">Entity</typeparam>
        /// <param name="key">Key</param>
        /// <param name="entities">An list</param>
        /// <param name="cacheTime">Cache time</param>
        /// <returns></returns>
        Task<bool> UpdateAsync<TEntity>(string key, IList<TEntity> entities, TimeSpan? cacheTime = null) where TEntity : class;
    }
}