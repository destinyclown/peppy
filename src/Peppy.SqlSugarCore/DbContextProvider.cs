using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.SqlSugarCore
{
    public class DbContextProvider : IDbContextProvider
    {
        private readonly IOptions<SqlSugarCoreOptions> _options;

        public DbContextProvider(IOptions<SqlSugarCoreOptions> options)
        {
            _options = options;
        }

        public SqlSugarCoreOptions GetOptions()
        {
            return _options.Value;
        }
    }
}