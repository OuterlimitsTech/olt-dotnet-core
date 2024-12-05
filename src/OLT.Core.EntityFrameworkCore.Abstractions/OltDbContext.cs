using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OLT.Constants;

namespace OLT.Core
{
    public abstract class OltDbContext<TContext> : DbContext, IOltDbContext
        where TContext: DbContext, IOltDbContext
    {
        private IOltDbAuditUser? _dbAuditUser;

#pragma warning disable S2743 // Static fields should not be used in generic types
#pragma warning disable IDE0044 // Add readonly modifier
        private static volatile OltNullableStringUtilities _nullableStringUtilities;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore S2743 // Static fields should not be used in generic types


#pragma warning disable S3963 // "static" fields should be initialized inline
        static OltDbContext()
        {
            _nullableStringUtilities = new OltNullableStringUtilities();
        }
#pragma warning restore S3963 // "static" fields should be initialized inline

        protected OltDbContext(DbContextOptions<TContext> options) : base(options)
        {
            
        }


        protected virtual IOltDbAuditUser DbAuditUser => _dbAuditUser ??= this.GetService<IOltDbAuditUser>();

        public abstract string DefaultSchema { get; }
        public abstract bool DisableCascadeDeleteConvention { get; }
        public virtual string DefaultAnonymousUser => OltEFCoreConstants.DefaultAnonymousUser;
        public abstract OltContextStringTypes DefaultStringType { get; }
        public virtual bool DisableAutomaticStringNullification => false;
        public abstract bool ApplyGlobalDeleteFilter { get; }

        public virtual string AuditUser
        {
            get
            {
                var userName = DbAuditUser?.GetDbUsername();
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    return userName;
                }
                return DefaultAnonymousUser;
            }
        }

        protected virtual AggregateException BuildException(Exception exception)
        {
            return OltContextExtensions.BuildException(this, exception);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {

            try
            {
                PrepareToSave();
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception)
            {                
                throw BuildException(exception);
            }
        }

        public override int SaveChanges()
        {
            try
            {
                PrepareToSave();
                return base.SaveChanges();
            }
            catch (Exception exception)
            {
                throw BuildException(exception);                
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            if (DisableCascadeDeleteConvention)
            {
                OltModelBuilderHelper.RestrictDeleteBehavior(modelBuilder);
            }

            if (DefaultStringType == OltContextStringTypes.Varchar)
            {
                OltModelBuilderHelper.DisableUnicodeProperties(modelBuilder);
            }

            if (!string.IsNullOrWhiteSpace(DefaultSchema))
            {
                modelBuilder.HasDefaultSchema(DefaultSchema);  //Sets Schema for all tables, unless overridden
            }

            OltModelBuilderHelper.EntityIdColumnName(modelBuilder);


            if (ApplyGlobalDeleteFilter)
            {
                //To Bypass 
                // https://docs.microsoft.com/en-us/ef/core/querying/filters#disabling-filters
                OltModelBuilderExtensions.SetSoftDeleteGlobalFilter(modelBuilder);                
            }

            base.OnModelCreating(modelBuilder);
        }



        #region [ Prep Save Methods ]

        protected virtual void PrepareToSave()
        {
            var entries = this.ChangeTracker.Entries().ToList();
            var changed = entries.Where(p => p.State == EntityState.Added || p.State == EntityState.Modified).ToList();

            foreach (var entry in changed)
            {
                SetAuditFields(entry);
                SetAbstractFields(entry);
                CallTriggers(entry);
                CheckNullableStringFields(entry);
            }
        }

        protected virtual void SetAuditFields(EntityEntry entityEntry)
        {
            OltEntityEntryExtensions.SetAuditFields(entityEntry, AuditUser);
        }

        protected virtual void SetAbstractFields(EntityEntry entityEntry)
        {
            OltEntityEntryExtensions.SetAbstractFields(entityEntry);
        }

        protected virtual void CallTriggers(EntityEntry entityEntry)
        {

            OltEntityEntryExtensions.CallTriggers(entityEntry, this);

            if (entityEntry.State == EntityState.Added)
            {
                (entityEntry.Entity as IOltInsertingRecord)?.InsertingRecord(this, entityEntry);
            }

            if (entityEntry.State == EntityState.Modified)
            {
                (entityEntry.Entity as IOltUpdatingRecord)?.UpdatingRecord(this, entityEntry);
            }

        }

        protected virtual void CheckNullableStringFields(EntityEntry entityEntry)
        {
            if (!DisableAutomaticStringNullification)
            {
                OltEntityEntryExtensions.CheckNullableStringFields(entityEntry, _nullableStringUtilities);
            }
        }



        #endregion

    }
}