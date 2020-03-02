using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.WebApi
{
    public enum StatusCodeEnum : int
    {
        /// <summary>
        /// 未授权访问
        /// </summary>
        [Description("未授权访问")]
        Unauthorized = 401,

        /// <summary>
        ///
        /// </summary>
        [Description("不允许访问")]
        Forbidden = 403,
    }
}