using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Peppy.Domain.UnitOfWork;
using Peppy.EntityFrameworkCore.UnitOfWork;
using System;

namespace Peppy.EntityFrameworkCore
{
    public class EFCoreOptionsExtension<TContext> : IPeppyOptionsExtension
        where TContext : DbContext
    {
        private readonly Action<EFCoreOptions> _configure;

        public EFCoreOptionsExtension(Action<EFCoreOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            var options = new EFCoreOptions();
            _configure(options);
            services.AddDbContext<TContext>();
            services.Configure(_configure);
            services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager<TContext>>();
            services.AddScoped<IUnitOfWorkCompleteHandle, UnitOfWorkCompleteHandle<TContext>>();
            services.AddScoped<IDbContextProvider<TContext>, DbContextProvider<TContext>>();
        }
    }
}