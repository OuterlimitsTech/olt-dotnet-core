using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;


// ReSharper disable once CheckNamespace
namespace OLT.Core
{

    public static class OltContextExtensions
    {
        public static void ProcessException(this DbContext context, Exception exception)
        {
            if (exception is DbUpdateException dbUpdateException)
            {
                WriteExceptionEntries(context, dbUpdateException.Entries);
            }
            else
            {
                WriteExceptionEntries(context, context.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged));
            }
        }

        public static void WriteExceptionEntries(this DbContext context, IEnumerable<EntityEntry> entries)
        {
            var entryDetails = new List<string>();
            var errors = new List<string>();
            foreach (var entry in entries)
            {
                foreach (var prop in entry.CurrentValues.Properties)
                {
                    if (prop?.PropertyInfo == null) continue;

                    var val = prop.PropertyInfo.GetValue(entry.Entity);
                    entryDetails.Add($"[DB Field] -> {context.ContextId}: {prop} ~ ({val?.ToString()?.Length}) - ({val})");
                    if (val?.ToString()?.Length > prop.GetMaxLength())
                    {
                        errors.Add($"[DB Field] MaxLength Exceeded -> {context.ContextId}: {prop} ----> ({val}) [{val?.ToString()?.Length} > {prop.GetMaxLength()}] <----");
                    }
                }
            }

            try
            {

                var logger = context.GetService<ILogger>();
                foreach (var detail in entryDetails)
                {
                    logger.LogDebug("DB Exception -> Entry Detail: {detail}", detail);
                }

                foreach (var error in errors)
                {
                    logger.LogError("DB Exception: {error}", error);
                }
            }
            catch (Exception)
            {
                var exceptions = errors.Select(s => new OltException(s));
                if (exceptions.Any())
                {
                    throw new AggregateException("[DB Field] MaxLength Exceeded", exceptions);
                }
            }

        }

        public static string GetTableName<TEntity>(this DbContext context) where TEntity : class
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var entityType = context.Model.FindEntityType(typeof(TEntity));
            var schema = entityType.GetSchema();
            var tableName = entityType.GetTableName();
            return string.IsNullOrEmpty(schema) ? tableName : $"{schema}.{tableName}";
        }

        public static IEnumerable<OltDbColumnInfo> GetColumns<TEntity>(this DbContext dbContext)
            where TEntity : class
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            var cols = new List<OltDbColumnInfo>();
            var entityType = dbContext.Model.FindEntityType(typeof(TEntity));

            // Table info 
            var tableName = entityType.GetTableName();
            var tableSchema = entityType.GetSchema();

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
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

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