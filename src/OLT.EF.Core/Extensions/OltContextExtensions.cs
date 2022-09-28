using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


// ReSharper disable once CheckNamespace
namespace OLT.Core
{

    public static class OltContextExtensions
    {
        
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