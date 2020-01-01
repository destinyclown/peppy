using StackExchange.Redis;
using System;

namespace Peppy.Redis
{
    public class StackExchangeRedisHelper
    {
        private static readonly object _locker = new object();
        private static StackExchangeRedisHelper _instance = null;
        private static PeppyRedisOptions _options = null;
        private static string _connStr = string.Empty;

        public static StackExchangeRedisHelper Instance()
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null || _instance._conn.IsConnected == false)
                    {
                        _instance = new StackExchangeRedisHelper(_options);
                    }
                }
            }
            return _instance;
        }

        private static void StackExchangeRedisRegistry(PeppyRedisOptions options)
        {
            _options = options;
            _connStr = string.Format("{0}:{1},allowAdmin=true,password={2},defaultdatabase={3}",
              _options.HostName,
              _options.Port,
              _options.Password,
              _options.Defaultdatabase
            );
        }

        private ConnectionMultiplexer _conn;

        /// <summary>
        /// 使用一个静态属性来返回已连接的实例，如下列中所示。这样，一旦 ConnectionMultiplexer 断开连接，便可以初始化新的连接实例。
        /// </summary>
        /// <param name="options"></param>
        public StackExchangeRedisHelper(PeppyRedisOptions options)
        {
            try
            {
                StackExchangeRedisRegistry(options);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"redis config: {_connStr}");
                Console.WriteLine($"redis link start");
                _conn = ConnectionMultiplexer.Connect(_connStr);
                Console.WriteLine("redis link success");
                Console.ResetColor();
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"redis error: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine();
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
                return _conn.GetDatabase();
            }
        }
    }
}