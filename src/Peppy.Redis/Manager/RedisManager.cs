using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peppy.Redis.Manager
{
    public class RedisManager : IRedisManager
    {
        private readonly ILogger<RedisManager> _logger;
        private readonly IOptions<RedisOptions> _redisOptions;
        private ConnectionMultiplexer _conn;
        private static string _connStr;

        public RedisManager(
            ILogger<RedisManager> logger,
            IOptions<RedisOptions> redisOptions)
        {
            _logger = logger;
            _redisOptions = redisOptions;
            _connStr = string.Format("{0}:{1},allowAdmin=true,password={2},defaultdatabase={3}",
              _redisOptions.Value.HostName,
              _redisOptions.Value.Port,
              _redisOptions.Value.Password,
              _redisOptions.Value.DefaultDatabase
            );
            RedisConnection();
        }

        public ConnectionMultiplexer RedisConnection()
        {
            try
            {
                _logger.LogDebug($"Redis config: {_connStr}");
                _conn = ConnectionMultiplexer.Connect(_connStr);
                _logger.LogInformation("Redis manager started!");
                return _conn;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Redis connection error: {ex.Message}");
                throw ex;
            }
        }

        public IDatabase GetDatabase()
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

        public bool Set<TEntity>(string key, TEntity entity, TimeSpan? cacheTime = null)
        {
            if (Exists(key))
            {
                Remove(key);
            }
            var result = GetDatabase().StringSet(key, JsonConvert.SerializeObject(entity));
            if (cacheTime != null)
            {
                return result && Expire(key, cacheTime.Value);
            }
            return result;
        }

        public bool Set<TEntity>(string key, TEntity[] entities, TimeSpan? cacheTime = null)
        {
            if (Exists(key))
            {
                Remove(key);
            }
            var redisValues = entities.Select(p => (RedisValue)(JsonConvert.SerializeObject(p))).ToArray();
            var result = GetDatabase().SetAdd(key, redisValues) == redisValues.Length;
            if (cacheTime != null)
            {
                return result && Expire(key, cacheTime.Value);
            }
            return result;
        }

        public bool Set<TEntity>(string key, List<TEntity> entities, TimeSpan? cacheTime = null)
        {
            if (Exists(key))
            {
                Remove(key);
            }
            return Set(key, entities.ToArray(), cacheTime);
        }

        public async Task<bool> SetAsync<TEntity>(string key, TEntity entity, TimeSpan? cacheTime = null)
        {
            if (await ExistsAsync(key))
            {
                await RemoveAsync(key);
            }
            var result = await GetDatabase().StringSetAsync(key, JsonConvert.SerializeObject(entity));
            if (cacheTime != null)
            {
                return result && await ExpireAsync(key, cacheTime.Value);
            }
            return result;
        }

        public async Task<bool> SetAsync<TEntity>(string key, TEntity[] entities, TimeSpan? cacheTime = null)
        {
            if (await ExistsAsync(key))
            {
                await RemoveAsync(key);
            }
            var redisValues = entities.Select(p => (RedisValue)(JsonConvert.SerializeObject(p))).ToArray();
            var result = await GetDatabase().SetAddAsync(key, redisValues) == redisValues.Length;
            if (cacheTime != null)
            {
                return result && await ExpireAsync(key, cacheTime.Value);
            }
            return result;
        }

        public async Task<bool> SetAsync<TEntity>(string key, List<TEntity> entities, TimeSpan? cacheTime = null)
        {
            if (await ExistsAsync(key))
            {
                await RemoveAsync(key);
            }
            return await SetAsync(key, entities.ToArray(), cacheTime);
        }

        public long Count(string key)
        {
            return GetDatabase().ListLength(key);
        }

        public async Task<long> CountAsync(string key)
        {
            return await GetDatabase().ListLengthAsync(key);
        }

        public bool Exists(string key)
        {
            return GetDatabase().KeyExists(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await GetDatabase().KeyExistsAsync(key);
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
            var redisKeys = keys.Select(p => (RedisKey)(JsonConvert.SerializeObject(p))).ToArray();
            return GetDatabase().KeyDelete(redisKeys) == redisKeys.Length;
        }

        public bool Remove(List<string> keys)
        {
            return Remove(keys.ToArray());
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await GetDatabase().KeyDeleteAsync(key);
        }

        public async Task<bool> RemoveAsync(string[] keys)
        {
            var redisKeys = keys.Select(p => (RedisKey)(JsonConvert.SerializeObject(p))).ToArray();
            return await GetDatabase().KeyDeleteAsync(redisKeys) == redisKeys.Length;
        }

        public async Task<bool> RemoveAsync(List<string> keys)
        {
            return await RemoveAsync(keys.ToArray());
        }

        public string BlockingDequeue(string key)
        {
            return GetDatabase().ListRightPop(key);
        }

        public async Task<string> BlockingDequeueAsync(string key)
        {
            return await GetDatabase().ListRightPopAsync(key);
        }

        public void Enqueue<TEntity>(string key, TEntity entity)
        {
            GetDatabase().ListLeftPush(key, JsonConvert.SerializeObject(entity));
        }

        public async Task EnqueueAsync<TEntity>(string key, TEntity entity)
        {
            await GetDatabase().ListLeftPushAsync(key, JsonConvert.SerializeObject(entity));
        }

        public long Increment(string key)
        {
            return GetDatabase().StringIncrement(key, flags: CommandFlags.FireAndForget);
        }

        public async Task<long> IncrementAsync(string key)
        {
            return await GetDatabase().StringIncrementAsync(key, flags: CommandFlags.FireAndForget);
        }

        public long Decrement(string key, string value)
        {
            return GetDatabase().HashDecrement(key, value, flags: CommandFlags.FireAndForget);
        }

        public async Task<long> DecrementAsync(string key, string value)
        {
            return await GetDatabase().HashDecrementAsync(key, value, flags: CommandFlags.FireAndForget);
        }

        public TEntity Get<TEntity>(string key)
        {
            if (!Exists(key))
            {
                return default;
            }
            var result = GetDatabase().StringGet(key);
            return JsonConvert.DeserializeObject<TEntity>(result);
        }

        public List<TEntity> GetList<TEntity>(string key)
        {
            if (!Exists(key))
            {
                return null;
            }
            var result = GetDatabase().SetMembers(key);
            return result.Select(p => JsonConvert.DeserializeObject<TEntity>(p)).ToList();
        }

        public TEntity[] GetArray<TEntity>(string key)
        {
            if (!Exists(key))
            {
                return null;
            }
            var result = GetDatabase().SetMembers(key);
            return result.Select(p => JsonConvert.DeserializeObject<TEntity>(p)).ToArray();
        }

        public async Task<TEntity> GetAsync<TEntity>(string key)
        {
            if (!await ExistsAsync(key))
            {
                return default;
            }
            var result = await GetDatabase().StringGetAsync(key);
            return JsonConvert.DeserializeObject<TEntity>(result);
        }

        public async Task<List<TEntity>> GetListAsync<TEntity>(string key)
        {
            if (!await ExistsAsync(key))
            {
                return null;
            }
            var result = await GetDatabase().SetMembersAsync(key);
            return result.Select(p => JsonConvert.DeserializeObject<TEntity>(p)).ToList();
        }

        public async Task<TEntity[]> GetArrayAsync<TEntity>(string key)
        {
            if (!await ExistsAsync(key))
            {
                return null;
            }
            var result = await GetDatabase().SetMembersAsync(key);
            return result.Select(p => JsonConvert.DeserializeObject<TEntity>(p)).ToArray();
        }
    }
}