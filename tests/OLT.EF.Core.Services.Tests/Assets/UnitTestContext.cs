using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System;
using System.Data.Common;
using System.Linq;
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

    public class UnitTestContext : OltDbContext<UnitTestContext>
    {
        public UnitTestContext(DbContextOptions<UnitTestContext> options) : base(options)
        {
        }

        public override string DefaultSchema => "UnitTest";
        public override bool DisableCascadeDeleteConvention => true;
        public override OltContextStringTypes DefaultStringType => OltContextStringTypes.Varchar;
        public override bool ApplyGlobalDeleteFilter => true;

        public virtual DbSet<PersonEntity> People { get; set; }
        public virtual DbSet<AddressEntity> Addresses { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }

        private DatabaseFacade _database;

        public override DatabaseFacade Database
        {
            get
            {
                return _database ??= new MockDatabaseFacade(this);
            }
        }
    }

}
