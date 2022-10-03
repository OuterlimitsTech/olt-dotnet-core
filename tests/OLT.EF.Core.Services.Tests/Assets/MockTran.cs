using Microsoft.EntityFrameworkCore.Storage;
using OLT.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OLT.EF.Core.Services.Tests.Assets
{
    public class MockTran : OltDisposable, IDbContextTransaction
    {
        public Guid TransactionId => Guid.NewGuid();

        public void Commit()
        {
            //Do Nothing
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
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
            //Do Nothing
        }

        public Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task CreateSavepointAsync(string name, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task ReleaseSavepointAsync(string name, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task RollbackToSavepointAsync(string name, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

    }

}
