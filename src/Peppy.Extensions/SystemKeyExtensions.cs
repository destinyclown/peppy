using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Extensions
{
    public static class SystemKeyExtensions
    {
        /// <summary>
        /// If extensions
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
        /// If extensions
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
        /// If extensions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="condition"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T If<T>(this T t, bool condition, Func<T, T> func) where T : class => condition ? func(t) : t;

        /// <summary>
        /// If extensions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="predicate"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T If<T>(this T t, Predicate<T> predicate, Func<T, T> func) where T : class => predicate(t) ? func(t) : t;

        /// <summary>
        /// If and else extensions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="condition"></param>
        /// <param name="ifAction"></param>
        /// <param name="elseAction"></param>
        /// <returns></returns>
        public static T IfAndElse<T>(this T t, bool condition, Action<T> ifAction, Action<T> elseAction) where T : class
        {
            if (condition)
            {
                ifAction(t);
            }
            else
            {
                elseAction(t);
            }

            return t;
        }

        /// <summary>
        /// If and else extensions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="predicate"></param>
        /// <param name="ifAction"></param>
        /// <param name="elseAction"></param>
        /// <returns></returns>
        public static T IfAndElse<T>(this T t, Predicate<T> predicate, Action<T> ifAction, Action<T> elseAction) where T : class
        {
            if (t == null)
            {
                throw new ArgumentNullException();
            }

            if (predicate(t))
            {
                ifAction(t);
            }
            else
            {
                elseAction(t);
            }

            return t;
        }

        /// <summary>
        /// If and else extensions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="condition"></param>
        /// <param name="ifFunc"></param>
        /// <param name="elseFunc"></param>
        /// <returns></returns>
        public static T IfAndElse<T>(this T t, bool condition, Func<T, T> ifFunc, Func<T, T> elseFunc) where T : class => condition ? ifFunc(t) : elseFunc(t);

        /// <summary>
        /// If and else extensions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="predicate"></param>
        /// <param name="ifFunc"></param>
        /// <param name="elseFunc"></param>
        /// <returns></returns>
        public static T IfAndElse<T>(this T t, Predicate<T> predicate, Func<T, T> ifFunc, Func<T, T> elseFunc) where T : class => predicate(t) ? ifFunc(t) : elseFunc(t);
    }
}