using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Peppy.Core;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peppy.Redis.Manager
{
    internal sealed class RedisManager : IRedisManager
    {
        private readonly ILogger<RedisManager> _logger;
        private readonly IOptions<PeppyRedisOptions> _redisOptions;
        private ConnectionMultiplexer _conn;
        private static string _connStr;

        public RedisManager(
            ILogger<RedisManager> logger,
            IOptions<PeppyRedisOptions> redisOptions)
        {
            _logger = logger;
            _redisOptions = redisOptions;
            _connStr = string.Format("{0}:{1},allowAdmin=true,password={2},defaultdatabase={3}",
              _redisOptions.Value.HostName,
              _redisOptions.Value.Port,
              _redisOptions.Value.Password,
              _redisOptions.Value.Defaultdatabase
            );
            RedisConnection();
        }

        private void RedisConnection()
        {
            try
            {
                _logger.LogDebug($"Redis config: {_connStr}");
                _conn = ConnectionMultiplexer.Connect(_connStr);
                _logger.LogInformation("Redis manager started!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Redis connection error: {ex.Message}");
            }
        }

        private IDatabase GetDatabase()
        {
            try
            {
                return _conn.GetDatabase();
            }
            catch
            {
                _conn = ConnectionMultiplexer.Connect(_connStr);
                _logger.LogInformation("Redis manager reconnection!");
                return _conn.GetDatabase();
            }
        }

        public bool Add<TEntity>(string key, TEntity entity, TimeSpan? cacheTime = null) where TEntity : class
        {
            var result = GetDatabase().SetAdd(key, entity.ToJson());
            if (cacheTime != null)
            {
                return result && Expire(key, cacheTime.Value);
            }
            return result;
        }

        public bool Add<TEntity>(string key, TEntity[] entities, TimeSpan? cacheTime = null) where TEntity : class
        {
            var redisValues = entities.Select(p => (RedisValue)p.ToJson()).ToArray();
            var result = GetDatabase().SetAdd(key, redisValues) == redisValues.Length;
            if (cacheTime != null)
            {
                return result && Expire(key, cacheTime.Value);
            }
            return result;
        }

        public bool Add<TEntity>(string key, IList<TEntity> entities, TimeSpan? cacheTime = null) where TEntity : class
        {
            return Add(key, entities.ToArray(), cacheTime);
        }

        public async Task<bool> AddAsync<TEntity>(string key, TEntity entity, TimeSpan? cacheTime = null) where TEntity : class
        {
            var result = await GetDatabase().SetAddAsync(key, entity.ToJson());
            if (cacheTime != null)
            {
                return result && await ExpireAsync(key, cacheTime.Value);
            }
            return result;
        }

        public async Task<bool> AddAsync<TEntity>(string key, TEntity[] entities, TimeSpan? cacheTime = null) where TEntity : class
        {
            var redisValues = entities.Select(p => (RedisValue)p.ToJson()).ToArray();
            var result = await GetDatabase().SetAddAsync(key, redisValues) == redisValues.Length;
            if (cacheTime != null)
            {
                return result && await ExpireAsync(key, cacheTime.Value);
            }
            return result;
        }

        public async Task<bool> AddAsync<TEntity>(string key, IList<TEntity> entities, TimeSpan? cacheTime = null) where TEntity : class
        {
            return await AddAsync(key, entities.ToArray(), cacheTime);
        }

        public long Count(string key)
        {
            return GetDatabase().ListLength(key);
        }

        public async Task<long> CountAsync(string key)
        {
            return await GetDatabase().ListLengthAsync(key);
        }

        public bool Expire(string key, TimeSpan cacheTime)
        {
            return GetDatabase().KeyExpire(key, DateTime.Now.AddSeconds(int.Parse(cacheTime.TotalSeconds.ToString())));
        }

        public async Task<bool> ExpireAsync(string key, TimeSpan cacheTime)
        {
            return await GetDatabase().KeyExpireAsync(key, DateTime.Now.AddSeconds(int.Parse(cacheTime.TotalSeconds.ToString())));
        }

        public bool Remove(string key)
        {
            return GetDatabase().KeyDelete(key);
        }

        public bool Remove(string[] keys)
        {
            var redisKeys = keys.Select(p => (RedisKey)p.ToJson()).ToArray();
            return GetDatabase().KeyDelete(redisKeys) == redisKeys.Length;
        }

        public bool Remove(IList<string> keys)
        {
            return Remove(keys.ToArray());
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await GetDatabase().KeyDeleteAsync(key);
        }

        public async Task<bool> RemoveAsync(string[] keys)
        {
            var redisKeys = keys.Select(p => (RedisKey)p.ToJson()).ToArray();
            return await GetDatabase().KeyDeleteAsync(redisKeys) == redisKeys.Length;
        }

        public async Task<bool> RemoveAsync(IList<string> keys)
        {
            return await RemoveAsync(keys.ToArray());
        }

        public bool Update<TEntity>(string key, TEntity entity, TimeSpan? cacheTime = null) where TEntity : class
        {
            return Add(key, entity, cacheTime);
        }

        public bool Update<TEntity>(string key, TEntity[] entities, TimeSpan? cacheTime = null) where TEntity : class
        {
            return Add(key, entities, cacheTime);
        }

        public bool Update<TEntity>(string key, IList<TEntity> entities, TimeSpan? cacheTime = null) where TEntity : class
        {
            return Add(key, entities, cacheTime);
        }

        public async Task<bool> UpdateAsync<TEntity>(string key, TEntity entity, TimeSpan? cacheTime = null) where TEntity : class
        {
            return await AddAsync(key, entity, cacheTime);
        }

        public async Task<bool> UpdateAsync<TEntity>(string key, TEntity[] entities, TimeSpan? cacheTime = null) where TEntity : class
        {
            return await AddAsync(key, entities, cacheTime);
        }

        public async Task<bool> UpdateAsync<TEntity>(string key, IList<TEntity> entities, TimeSpan? cacheTime = null) where TEntity : class
        {
            return await AddAsync(key, entities, cacheTime);
        }
    }
}