using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;


// ReSharper disable once CheckNamespace
namespace OLT.Core
{

    public static class OltContextExtensions
    {
        public static AggregateException BuildException(this DbContext context, Exception exception)
        {
            if (exception is DbUpdateException dbUpdateException)
            {
                return BuildException(context, exception, dbUpdateException.Entries);
            }
            else
            {
                return BuildException(context, exception, context.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged));
            }
        }

        public static AggregateException BuildException(this DbContext context, Exception exception, IEnumerable<EntityEntry> entries)
        {
            var errors = new List<Exception>
            {
                exception
            };

            foreach (var entry in entries)
            {
                foreach (var prop in entry.CurrentValues.Properties)
                {
                    if (prop?.PropertyInfo == null) continue;

                    try
                    {                        
                        var val = prop.PropertyInfo.GetValue(entry.Entity);                                                
                        if (val?.ToString()?.Length > prop.GetMaxLength())
                        {
                            errors.Add(new ApplicationException($"[DB Field] MaxLength Exceeded -> {context.ContextId}: {prop?.Name} = [{val}] ---> {val?.ToString()?.Length} > {prop?.GetMaxLength()} <--"));
                        }
                        else
                        {
                            errors.Add(new ApplicationException($"[DB Field] -> {context.ContextId}: {prop?.Name} = [{val}]"));
                        }
                    }
                    catch { }
                }
            }         

            return new AggregateException(exception.Message, errors);
        }

        public static string? GetTableName<TEntity>(this DbContext context) where TEntity : class
        {
            ArgumentNullException.ThrowIfNull(context);
            var entityType = context.Model.FindEntityType(typeof(TEntity));
            var schema = entityType?.GetSchema();
            var tableName = entityType?.GetTableName();            
            return string.IsNullOrEmpty(schema) ? tableName : $"{schema}.{tableName}";
        }

        /// <summary>
        /// Get Columns 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static IEnumerable<OltDbColumnInfo> GetColumns<TEntity>(this DbContext dbContext)
            where TEntity : class
        {
            ArgumentNullException.ThrowIfNull(dbContext);

            var cols = new List<OltDbColumnInfo>();
            var entityType = dbContext.Model.FindEntityType(typeof(TEntity));

            if (entityType == null) throw new InvalidOperationException("Unable to locate entity type");

            // Table info 
            var tableName = entityType.GetTableName();
            var tableSchema = entityType.GetSchema();

            if (tableName == null) throw new InvalidOperationException("Unable locate table name");

            // Column info 
            foreach (var property in entityType.GetProperties())
            {
                var column = new OltDbColumnInfo
                {
                    Name = property.GetColumnName(StoreObjectIdentifier.Table(tableName, tableSchema)),
                };

                try
                {
                    column.Type = property.GetColumnType();
                }
                catch
                {
                    column.Type = property.GetTypeMapping().ClrType.Name;
                }
                cols.Add(column);
            }

            return cols;
        }

        public static IQueryable<TEntity> InitializeQueryable<TEntity>(this IOltDbContext context)
            where TEntity : class, IOltEntity
        {
            return InitializeQueryable<TEntity>(context, true);
        }

        public static IQueryable<TEntity> InitializeQueryable<TEntity>(this IOltDbContext context, bool includeDeleted)
            where TEntity : class, IOltEntity
        {
            ArgumentNullException.ThrowIfNull(context);

            var query = context.Set<TEntity>().AsQueryable();
            if (context.ApplyGlobalDeleteFilter)
            {
                if (includeDeleted)
                {
                    query = query.IgnoreQueryFilters();
                }
            }
            else if (!includeDeleted)
            {
                query = OltGeneralQueryableExtensions.NonDeletedQueryable(query);
            }
            return query;
        }


    }
}