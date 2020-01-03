using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Peppy.Core
{
    public static class Extensions
    {
        /// <summary>
        /// 类对像转换成json格式
        /// </summary>
        /// <returns></returns>
        public static string ToJson(this object t)
        {
            var ser = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, };
            ser.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd hh:mm:ss" });
            return JsonConvert.SerializeObject(t, Formatting.Indented, ser);
        }

        /// <summary>
        /// 类转化为json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T t)
        {
            var ser = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, };
            ser.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd hh:mm:ss" });
            return JsonConvert.SerializeObject(t, Formatting.Indented, ser);
        }

        /// <summary>
        /// 类对像转换成json格式
        /// </summary>
        /// <param name="t"></param>
        /// <param name="HasNullIgnore">是否忽略NULL值</param>
        /// <returns></returns>
        public static string ToJson(this object t, bool HasNullIgnore)
        {
            if (HasNullIgnore)
            {
                var ser = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                ser.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd hh:mm:ss" });
                return JsonConvert.SerializeObject(t, Formatting.Indented, ser);
            }
            else
                return ToJson(t);
        }
    }
}