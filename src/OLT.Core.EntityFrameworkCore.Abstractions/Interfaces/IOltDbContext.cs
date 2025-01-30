using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OLT.Core
{
    public interface IOltDbContext : IDisposable
    {
        string DefaultAnonymousUser { get; }
        string AuditUser { get; }
        bool ApplyGlobalDeleteFilter { get; }
        DatabaseFacade Database { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }

}