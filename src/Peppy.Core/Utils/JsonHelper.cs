using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;

namespace Peppy.Core
{
    public static class JsonHelper
    {
        #region Method

        /// <summary>
        /// json格式转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T FromJson<T>(string strJson) where T : class
        {
            if (!string.IsNullOrEmpty(strJson))
                return JsonConvert.DeserializeObject<T>(strJson);
            return null;
        }

        /// <summary>
        /// json格式转换成List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static List<T> FromJsonList<T>(string strJson) where T : class
        {
            if (!string.IsNullOrEmpty(strJson))
                return JsonConvert.DeserializeObject<List<T>>(strJson);
            return null;
        }

        /// <summary>
        /// 字符串转成匿名对象
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static JObject Parse(string strJson)
        {
            if (string.IsNullOrEmpty(strJson))
                return null;
            JObject o = JObject.Parse(strJson);
            return o;
        }

        #endregion Method

        /// <summary>
        /// json字符串转化成json数组
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static JArray ParsestrToJArray(string strJson)
        {
            if (string.IsNullOrEmpty(strJson))
                return new JArray();
            JArray array = (JArray)JsonConvert.DeserializeObject(strJson);
            return array;
        }

        //public static object ToJson(this string Json)
        //{
        //    return Json == null ? null : JsonConvert.DeserializeObject(Json);
        //}

        public static string ToJson(this object obj, string datetimeformats)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformats };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

        public static T ToObject<T>(this string Json)
        {
            return Json == null ? default(T) : JsonConvert.DeserializeObject<T>(Json);
        }

        public static List<T> ToList<T>(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<List<T>>(Json);
        }

        public static DataTable ToTable(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<DataTable>(Json);
        }

        public static JObject ToJObject(this string Json)
        {
            return Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
        }
    }
}