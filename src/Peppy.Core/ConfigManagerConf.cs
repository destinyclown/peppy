using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Peppy.Core
{
    public static class ConfigManagerConf
    {
        public static IConfiguration Configuration = null;

        static readonly ConcurrentDictionary<string, List<string>> _dicCache = new ConcurrentDictionary<string, List<string>>();



        public static void SetConfiguration(IConfiguration configuration)
        {
            if (Configuration == null)
                Configuration = configuration;

        }

        /// <summary>
        /// 值类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            _dicCache.TryGetValue(key, out List<string> refValue);
            if (refValue != null)
                return refValue?[0];

            if (Configuration == null)
                return "";
            string value = Configuration[key];
            if (!string.IsNullOrEmpty(value)) //本地存在则返回
                return value;
            return "";
        }

        /// <summary>
        /// 引用类型(开发建议使用) List<string> list = ConfigManager.GetReferenceValue("") 使用List 即可
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> GetReferenceValue(string key)
        {
            _dicCache.TryGetValue(key, out List<string> refValue);
            if (refValue == null)
            {
                refValue = new List<string>
                {
                    GetValue(key)//[0]
                };
                _dicCache.TryAdd(key, refValue);
            }
            return refValue;
        }

    }
}
