using Microsoft.EntityFrameworkCore;
using Peppy.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Peppy.EntityFrameworkCore.UnitOfWork
{
    public class UnitOfWorkCompleteHandle<TDbContext> : IUnitOfWorkCompleteHandle
        where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public UnitOfWorkCompleteHandle(TDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void Complete()
        {
            _dbContext.SaveChanges();
        }

        public async Task CompleteAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}