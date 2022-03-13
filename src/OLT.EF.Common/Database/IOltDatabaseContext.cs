using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OLT.Core
{
    public interface IOltDatabaseContext : IDisposable
    {
        string DefaultAnonymousUser { get; }
        string AuditUser { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }


    public interface IOltDatabaseContext<out T> : IOltDatabaseContext
        where T : class
    {
        T Database { get; }
    }

}
