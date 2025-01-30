using Microsoft.EntityFrameworkCore;

namespace OLT.Core;

public abstract class OltSqlDbContext<TContext> : OltDbContext<TContext>
    where TContext : DbContext, IOltDbContext
{
    protected OltSqlDbContext(DbContextOptions<TContext> options) : base(options)
    {

    }

    protected abstract int IdentitySeed { get; }
    protected abstract int IdentityIncrement { get; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        OltSqlModelBuilderExtensions.SetIdentityColumns(modelBuilder, IdentitySeed, IdentityIncrement);
    }
}


