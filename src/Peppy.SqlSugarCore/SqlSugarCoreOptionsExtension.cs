using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.SqlSugarCore
{
    public class SqlSugarCoreOptionsExtension : IPeppyOptionsExtension
    {
        private readonly Action<SqlSugarCoreOptions> _configure;

        public SqlSugarCoreOptionsExtension(Action<SqlSugarCoreOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            var options = new SqlSugarCoreOptions();
            _configure(options);
            services.Configure(_configure);
            services.AddScoped<IDbContextProvider, DbContextProvider>();
        }
    }
}