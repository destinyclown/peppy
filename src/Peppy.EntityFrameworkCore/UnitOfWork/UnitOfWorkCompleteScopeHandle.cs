using Peppy.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Peppy.EntityFrameworkCore.UnitOfWork
{
    public class UnitOfWorkCompleteScopeHandle<TDbContext> : IUnitOfWorkCompleteHandle
        where TDbContext : EFCroeDbContext
    {
        private readonly TransactionScope _transactionScope;

        public UnitOfWorkCompleteScopeHandle(TransactionScope transactionScope)
        {
            _transactionScope = transactionScope ?? throw new ArgumentNullException(nameof(transactionScope));
        }

        public void Complete()
        {
            _transactionScope.Complete();
        }

        public async Task CompleteAsync()
        {
            await Task.Run(() => _transactionScope.Complete());
        }

        public void Rollback()
        {
            _transactionScope.Dispose();
        }

        public async Task RollbackAsync()
        {
            await Task.Run(() => _transactionScope.Dispose());
        }

        public void Dispose()
        {
            _transactionScope.Dispose();
        }
    }
}
