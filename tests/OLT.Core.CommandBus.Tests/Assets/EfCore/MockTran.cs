using Microsoft.EntityFrameworkCore.Storage;
using OLT.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OLT.Core.CommandBus.Tests.Assets.EfCore
{

    public class MockTran : OltDisposable, IDbContextTransaction
    {
        public Guid TransactionId => Guid.NewGuid();
        public ConcurrentQueue<Guid> Transactions { get; set; } = new ConcurrentQueue<Guid>();

        public MockTran()
        {
            Transactions.Enqueue(Guid.NewGuid());
        }

        public void Commit()
        {
            Transactions.TryDequeue(out Guid result);
            //Do Nothing
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            Transactions.TryDequeue(out Guid result);
            //Do Nothing
            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            //Do Nothing
            await Task.Yield();
        }

        public void Rollback()
        {
            Transactions.TryDequeue(out Guid result);
            //Do Nothing
        }

        public Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            Transactions.TryDequeue(out Guid result);
            return Task.CompletedTask;
        }

        public Task CreateSavepointAsync(string name, CancellationToken cancellationToken = default)
        {
            Transactions.Enqueue(Guid.NewGuid());
            return Task.CompletedTask;
        }

        public Task ReleaseSavepointAsync(string name, CancellationToken cancellationToken = default)
        {
            Transactions.TryDequeue(out Guid result);
            return Task.CompletedTask;
        }

        public Task RollbackToSavepointAsync(string name, CancellationToken cancellationToken = default)
        {
            Transactions.TryDequeue(out Guid result);
            return Task.CompletedTask;
        }

    }
}
