using Peppy.Domain.Entities;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Peppy.EntityFrameworkCore.Repositories
{
    public class BaseRepository<TDbContext> : IBaseRepository<TDbContext>
        where TDbContext : EFCroeDbContext
    {
        private readonly IServiceProvider _serviceProvider;
        public BaseRepository(
            IServiceProvider serviceProvider
            )
        {
            _serviceProvider = serviceProvider;
        }

        public IRepositoryBase<TDbContext, TEntity, long> GetRepository<TEntity>()
            where TEntity : class, IEntity<long>
            => _serviceProvider.GetService<IRepositoryBase<TDbContext, TEntity, long>>();

        public IRepositoryBase<TDbContext, TEntity, TPrimaryKey> GetRepository<TEntity, TPrimaryKey>()
            where TEntity : class, IEntity<TPrimaryKey>
            => _serviceProvider.GetService<IRepositoryBase<TDbContext, TEntity, TPrimaryKey>>();
    }
}
