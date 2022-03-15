using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OLT.Core
{
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
            OltSqlModelBuilderHelper.SetIdentityColumns(modelBuilder, IdentitySeed, IdentityIncrement);
            base.OnModelCreating(modelBuilder);
        }
    }

   
}
