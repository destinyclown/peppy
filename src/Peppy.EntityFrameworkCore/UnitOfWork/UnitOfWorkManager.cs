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
            var tran = _dbContext.Database.BeginTransaction();
            var handle = new UnitOfWorkCompleteHandle<TDbContext>(tran);
            return handle;
        }

        public IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope)
        {
            return Begin(new UnitOfWorkOptions { Scope = scope });
        }

        public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
        {
            var scope = new TransactionScope(
                options.Scope.GetValueOrDefault(), 
                new TransactionOptions { 
                    Timeout = options.Timeout.GetValueOrDefault(),
                    IsolationLevel = options.IsolationLevel.GetValueOrDefault() 
                });
            var handle = new UnitOfWorkCompleteScopeHandle<TDbContext>(scope);
            return handle;
        }
    }
}