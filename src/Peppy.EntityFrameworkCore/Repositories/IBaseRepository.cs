using Peppy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.EntityFrameworkCore.Repositories
{
    public interface IBaseRepository<TDbContext>
        where TDbContext : EFCroeDbContext
    {
        IRepositoryBase<TDbContext, TEntity, long> GetRepository<TEntity>()
            where TEntity : class, IEntity<long>;

        IRepositoryBase<TDbContext, TEntity, TPrimaryKey> GetRepository<TEntity, TPrimaryKey>()
            where TEntity : class, IEntity<TPrimaryKey>;
    }
}
