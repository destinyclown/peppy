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
    }
}