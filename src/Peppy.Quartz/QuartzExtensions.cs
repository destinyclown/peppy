using Quartz;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Quartz
{
    internal static class QuartzExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="condition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static T If<T>(this T t, bool condition, Action<T> action) where T : class
        {
            if (condition)
            {
                action(t);
            }

            return t;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="predicate"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static T If<T>(this T t, Predicate<T> predicate, Action<T> action) where T : class
        {
            if (t == null)
            {
                throw new ArgumentNullException();
            }

            if (predicate(t))
            {
                action(t);
            }

            return t;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="predicate"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T If<T>(this T t, Predicate<T> predicate, Func<T, T> func) where T : struct => predicate(t) ? func(t) : t;

        /// <summary>
        /// add job data
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public static void AddJobData(this TriggerBuilder trigger, string key, string value, Type type)
        {
            if (type.FullName == null) return;
            switch (type.FullName.ToLower())
            {
                case "int":
                    trigger.UsingJobData(key, int.Parse(value));
                    break;

                case "long":
                    trigger.UsingJobData(key, long.Parse(value));
                    break;

                case "float":
                    trigger.UsingJobData(key, float.Parse(value));
                    break;

                case "double":
                    trigger.UsingJobData(key, double.Parse(value));
                    break;

                case "decimal":
                    trigger.UsingJobData(key, decimal.Parse(value));
                    break;

                case "bool":
                    trigger.UsingJobData(key, bool.Parse(value));
                    break;

                default:
                    trigger.UsingJobData(key, value);
                    break;
            }
        }
    }
}