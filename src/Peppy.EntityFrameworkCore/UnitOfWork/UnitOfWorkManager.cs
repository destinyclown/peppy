using Microsoft.EntityFrameworkCore;
using Peppy.Domain.UnitOfWork;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Peppy.EntityFrameworkCore.UnitOfWork
{
    public class UnitOfWorkManager<TDbContext> : IUnitOfWorkManager
        where TDbContext : EFCroeDbContext
    {
        private readonly TDbContext _dbContext;

        public UnitOfWorkManager(TDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IUnitOfWorkCompleteHandle Begin()
        {
            _dbContext.Database.BeginTransaction();
            var handle = new UnitOfWorkCompleteHandle<TDbContext>(_dbContext);
            return handle;
        }

        public IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope)
        {
            _dbContext.Database.BeginTransaction();
            var handle = new UnitOfWorkCompleteHandle<TDbContext>(_dbContext);
            return handle;
        }

        public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
        {
            _dbContext.Database.BeginTransaction();
            var handle = new UnitOfWorkCompleteHandle<TDbContext>(_dbContext);
            return handle;
        }
    }
}