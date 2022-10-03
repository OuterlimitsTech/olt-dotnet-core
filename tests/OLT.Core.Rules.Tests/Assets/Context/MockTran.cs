using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OLT.Core.Rules.Tests.Assets.Context
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

    //public class UnitTestContext : OltDbContext<UnitTestContext>
    //{
    //    public UnitTestContext(DbContextOptions<UnitTestContext> options) : base(options)
    //    {
    //    }

    //    public override string DefaultSchema => "UnitTest";
    //    public override bool DisableCascadeDeleteConvention => true;
    //    public override OltContextStringTypes DefaultStringType => OltContextStringTypes.Varchar;
    //    public override bool ApplyGlobalDeleteFilter => true;

    //    public virtual DbSet<PersonEntity> People { get; set; }


    //}


    //public class PersonEntity : OltEntityIdDeletable, IOltEntityUniqueId
    //{
    //    public Guid UniqueId { get; set; }

    //    public string NameFirst { get; set; }

    //    public string NameMiddle { get; set; }

    //    public string NameLast { get; set; }
    //}
}
