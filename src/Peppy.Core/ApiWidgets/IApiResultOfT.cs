using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Core
{
    public interface IApiResult<TResult> : IApiResult
    {

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        TResult Result { get; set; }
    }
}
