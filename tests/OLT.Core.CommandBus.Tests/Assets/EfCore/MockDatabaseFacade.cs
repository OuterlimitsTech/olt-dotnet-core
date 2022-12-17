using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace OLT.Core.CommandBus.Tests.Assets.EfCore
{
    public class MockDatabaseFacade : DatabaseFacade
    {
        private MockTran _transaction = null;

        public MockDatabaseFacade(DbContext context) : base(context)
        {
        }

        public override IDbContextTransaction CurrentTransaction
        {
            get
            {
                return _transaction;
            }
        }

        public override Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = new MockTran();
            return Task.FromResult<IDbContextTransaction>(_transaction);
        }

        public override Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = null;
            return Task.CompletedTask;
        }
        public override Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = null;
            return Task.CompletedTask;
        }
    }
}
