using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using OLT.Constants;

namespace OLT.Core
{
    public abstract class OltDbContext<TContext> : DbContext, IOltDbContext
        where TContext: DbContext, IOltDbContext
    {
        private IOltDbAuditUser _dbAuditUser;
        private ILogger<OltDbContext<TContext>> _logger;

#pragma warning disable S2743 // Static fields should not be used in generic types
#pragma warning disable IDE0044 // Add readonly modifier
        private static volatile object _entityMetatdataCacheSyncRoot;
        private static volatile Dictionary<RuntimeTypeHandle, List<NullableStringPropertyMetaData>> _entityMetatdataCache;
        private static volatile string _stringTypeName;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore S2743 // Static fields should not be used in generic types


#pragma warning disable S3963 // "static" fields should be initialized inline
        static OltDbContext()
        {
            _entityMetatdataCacheSyncRoot = new object();
            _stringTypeName = nameof(String);
            _entityMetatdataCache = new Dictionary<RuntimeTypeHandle, List<NullableStringPropertyMetaData>>();
        }
#pragma warning restore S3963 // "static" fields should be initialized inline

        protected OltDbContext(DbContextOptions<TContext> options) : base(options)
        {

        }


        protected virtual IOltDbAuditUser DbAuditUser => _dbAuditUser ??= this.GetService<IOltDbAuditUser>();
        protected virtual ILogger<OltDbContext<TContext>> Logger => _logger ??= this.GetService<ILogger<OltDbContext<TContext>>>();

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
                var userName = DbAuditUser.GetDbUsername();
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    return userName;
                }
                return DefaultAnonymousUser;
            }
        }

        protected virtual void ProcessException(Exception exception)
        {
            if (exception is DbUpdateException dbUpdateException)
            {
                WriteExceptionEntries(dbUpdateException.Entries);
            }
            else
            {
                WriteExceptionEntries(this.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged));
            }
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
                ProcessException(exception);
                throw;
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
                ProcessException(exception);
                throw;
            }
        }


        protected virtual void WriteExceptionEntries(IEnumerable<EntityEntry> entries)
        {

            foreach (var entry in entries)
            {
                foreach (var prop in entry.CurrentValues.Properties)
                {
                    var val = prop.PropertyInfo.GetValue(entry.Entity);
                    Logger.LogDebug("[DB Field] -> {identifier}: {propertyInfo} ~ ({valueLength}) - ({value})", ContextId, prop, val?.ToString().Length, val);

                    if (val?.ToString().Length > prop.GetMaxLength())
                    {
                        Logger.LogCritical("[DB Field] MaxLength Exceeded -> {identifier}: {propertyInfo} ----> ({value}) [{valueLength} > {maxLength}] <----", ContextId, prop, val, val?.ToString().Length, prop.GetMaxLength());
                    }
                }
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
            if (entityEntry.Entity is IOltEntityAudit createModel)
            {
                var utcNow = DateTimeOffset.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {                    
                    createModel.CreateUser = AuditUser;
                    createModel.CreateDate = utcNow;
                }

                createModel.ModifyUser = AuditUser;
                createModel.ModifyDate = utcNow;

            }
        }

        protected virtual void SetAbstractFields(EntityEntry entityEntry)
        {         
            if (entityEntry.Entity is IOltEntityUniqueId uniqueModel && uniqueModel.UniqueId == Guid.Empty)
            {
                uniqueModel.UniqueId = Guid.NewGuid();
            }

            if (entityEntry.Entity is IOltEntitySortable sortOrder && sortOrder.SortOrder <= 0)
            {
                sortOrder.SortOrder = OltCommonDefaults.SortOrder;
            }
        }

        protected virtual void CallTriggers(EntityEntry entityEntry)
        {

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
                var nullableStringFields = GetNullableStringPropertyMetaData(entityEntry);

                foreach (var nullableStringField in nullableStringFields)
                {
                    try
                    {
                        var currentValue = nullableStringField.GetValue(entityEntry);
                        if (currentValue == null) continue;
                        if (string.IsNullOrWhiteSpace(currentValue))
                        {
                            nullableStringField.SetToNullValue(entityEntry);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogCritical(ex, "Entity Type: {EntityType} -> {PropertyName}", entityEntry.Entity.GetType().FullName, nullableStringField.PropertyName);
                        throw new OltException($"CheckNullableStringFields: {entityEntry.Entity.GetType().FullName} -> {nullableStringField.PropertyName}", ex);
                    }
                }
            }
        }



        #endregion

        #region [ NullableStringPropertyMetaData ]

        private sealed class NullableStringPropertyMetaData
        {
            public EntityEntry EntityEntry { get; set; }
            public string PropertyName { get; set; }
            public MethodInfo Getter { get; set; }
            public MethodInfo Setter { get; set; }


            // Note - we use the GetGetter approach because EF may be a detached poco, dynamic proxy, or dynamic object.  
            // Simply using GetValue off PropertyInfo on a dynamic object will fail (same is true in EF Core).
            public string GetValue(EntityEntry source)
            {
                // Guard
                if (source == null) return null;

                if (this.Getter == null) return null;

                var sourceValue = this.Getter.Invoke(source.Entity, Array.Empty<object>()) as string;
                return sourceValue;
            }

            // Note - we use the GetSetter approach because EF may be a detached poco, dynamic proxy, or dynamic object.
            // Simply using SetValue off PropertyInfo on a dynamic object will fail (same is true in EF Core).
            public void SetToNullValue(EntityEntry source)
            {
                // Guard
                if (source == null) return;

                this.Setter.Invoke(source.Entity, new object[] { null });
            }
        }

        #endregion

        #region [ GetNullableStringPropertyMetaData ]

#pragma warning disable CA1822

        // Note - this looks in the thread-safe static cache to avoid the repetitive reflection.  Especially important since these things are
        // used in a VERY tight loop....
        private List<NullableStringPropertyMetaData> GetNullableStringPropertyMetaData(EntityEntry entry)
        {
            var type = entry.Entity.GetType();
            var typeHandle = type.TypeHandle;

            // Fast return if we did this already.....
            if (_entityMetatdataCache.ContainsKey(typeHandle))
            {
                return _entityMetatdataCache[typeHandle];
            }

            List<NullableStringPropertyMetaData> result = new List<NullableStringPropertyMetaData>();

            // Limit to public instance properties - that is the EF contract and we can't go around it without 
            // mucking up EF & EF Core internals....especially due to its internals on the contract for private backing fields.
            var stringProperties = entry.Entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                  .Where(p => p.PropertyType?.Name == _stringTypeName)    // Not really a "safe" comparison, but we go by typename rather than typeof to avoid language version issues.....
                  .ToList();

            // No string properties found found?
            if (stringProperties.Count == 0)
            {
                // Critical section for thread-safe implementation....
                lock (_entityMetatdataCacheSyncRoot)
                {
                    _entityMetatdataCache[typeHandle] = result;
                }

                return result;
            }

            foreach (var item in stringProperties)
            {
                // Check for things to skip...

                // Skip unwriteable properties
                if (!item.CanWrite) continue;

                // Get the set method - see comments on private class NullableStringPropertyMetaData for why we need to use this approach.
                var setter = item.GetSetMethod(true);  // Include private setters for proxies

                // Skip items with no setter
                if (setter == null)
                    // Funny thing about properties - they can be CanWrite = true but have no set method.                    
                    // This is a guard against that occurence, else we would blow up on setting a value
                    // even if CanWrite = true.
                    continue;

                // Skip NotMapped properties - NOTE: we use the static Attribute.GetCustomAttribute to make sure we walk the inheritance tree
                var notMappedAttribute = Attribute.GetCustomAttribute(item, typeof(NotMappedAttribute));
                if (notMappedAttribute != null) continue;

                // Skip Required properties. They cannot be nulled. NOTE: we use the static Attribute.GetCustomAttribute to make sure we walk the inheritance tree
                var requiredAttribute = Attribute.GetCustomAttribute(item, typeof(RequiredAttribute));
                if (requiredAttribute != null) continue;

                // If we get here then it's one we that we can safely null out...

                // Get the get method - see comments on private class NullableStringPropertyMetaData for why we need to use this approach.
                var getter = item.GetGetMethod(false);  // Public getters only please...this is EF!

                var info = new NullableStringPropertyMetaData
                {
                    EntityEntry = entry,
                    PropertyName = item.Name,
                    Getter = getter,
                    Setter = setter
                };

                result.Add(info);
            }

            // Critical section for thread-safe implementation....
            lock (_entityMetatdataCacheSyncRoot)
            {
                _entityMetatdataCache[typeHandle] = result;
            }

            return result;
        }

#pragma warning restore CA1822

        #endregion

    }
}