using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Peppy.Domain.UnitOfWork;
using Peppy.EntityFrameworkCore.UnitOfWork;
using System;

namespace Peppy.EntityFrameworkCore
{
    public class EFCoreOptionsExtension<TDbContext> : IPeppyOptionsExtension
        where TDbContext : EFCroeDbContext
    {
        private readonly Action<EFCoreOptions<TDbContext>> _configure;

        public EFCoreOptionsExtension(Action<EFCoreOptions<TDbContext>> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            var options = new EFCoreOptions<TDbContext>();
            _configure(options);
            services.AddDbContext<TDbContext>();
            services.Configure(_configure);
            services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager<TDbContext>>();
            services.AddScoped<IUnitOfWorkCompleteHandle, UnitOfWorkCompleteHandle<TDbContext>>();
            services.AddScoped<IDbContextProvider<TDbContext>, DbContextProvider<TDbContext>>();
        }
    }
}