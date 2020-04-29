using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Peppy.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Peppy.EntityFrameworkCore.UnitOfWork
{
    public class UnitOfWorkCompleteHandle<TDbContext> : IUnitOfWorkCompleteHandle
        where TDbContext : EFCroeDbContext
    {
        private readonly IDbContextTransaction _dbContextTransaction;

        public UnitOfWorkCompleteHandle(IDbContextTransaction dbContextTransaction)
        {
            _dbContextTransaction = dbContextTransaction ?? throw new ArgumentNullException(nameof(dbContextTransaction));
        }

        public void Complete()
        {
            _dbContextTransaction.Commit();
        }

        public async Task CompleteAsync()
        {
            await _dbContextTransaction.CommitAsync();
        }

        public void Rollback()
        {
            _dbContextTransaction.Rollback();
        }

        public async Task RollbackAsync()
        {
            await _dbContextTransaction.RollbackAsync();
        }

        public void Dispose()
        {
            _dbContextTransaction.Dispose();
        }
    }
}