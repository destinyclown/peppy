using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Microsoft.Extensions.Logging
{
    /// <summary>
    /// sql log extension
    /// </summary>
    public static class SqlLoggerExtension
    {
        /// <summary>
        /// write a sql log
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sql"></param>
        public static void LogSql(this ILogger logger, string sql)
        {
            SqlLoggerUtil.LogSql(sql);
        }

        /// <summary>
        /// write a sql log
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        public static void LogSql(this ILogger logger, string sql, DbParameter[] parameters)
        {
            SqlLoggerUtil.LogSql(sql, parameters);
        }
    }

    /// <summary>
    /// sql log util
    /// </summary>
    public static class SqlLoggerUtil
    {
        /// <summary>
        /// write a sql log
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="time"></param>
        public static void LogSql(string sql, TimeSpan time = default)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("sql: ");
            Console.ResetColor();
            if (time != default)
            {
                Console.WriteLine($"Executed SQL ({time.TotalMilliseconds.ToString("0")}ms)");
            }
            Console.WriteLine($"     {sql}");
            Console.WriteLine();
        }

        /// <summary>
        /// write a sql log
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="time"></param>
        public static void LogSql(string sql, DbParameter[] parameters, TimeSpan time = default)
        {
            var parstr = new StringBuilder();
            foreach (var parameter in parameters.ToDictionary(it => it.ParameterName, it => it.Value))
            {
                parstr.Append("[");
                parstr.Append(parameter.Key);
                parstr.Append("=");
                parstr.Append(parameter.Value);
                parstr.Append("]");
                parstr.Append(";");
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("sql: ");
            Console.ResetColor();
            if (time != default)
            {
                Console.Write($"Executed SQL ({time.TotalMilliseconds.ToString("0")}ms) ");
            }
            Console.Write($"[Parameters:{parstr.ToString()}]");
            Console.WriteLine();
            Console.WriteLine($"     {sql}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
    }
}