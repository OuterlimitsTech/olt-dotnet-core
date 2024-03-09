using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;

namespace OLT.Core
{
    public static class OltModelBuilderExtensions
    {

        public static ModelBuilder EntitiesOfType<T>(this ModelBuilder modelBuilder, Action<EntityTypeBuilder> buildAction) 
        {
            return EntitiesOfType(modelBuilder, typeof(T), buildAction);
        }

        public static ModelBuilder EntitiesOfType(this ModelBuilder modelBuilder, Type type, Action<EntityTypeBuilder> buildAction)
        {         
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (buildAction == null)
            {
                throw new ArgumentNullException(nameof(buildAction));
            }

            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(entityType => type.IsAssignableFrom(entityType.ClrType)))
            {
                buildAction(modelBuilder.Entity(entityType.ClrType));
            }

            return modelBuilder;

        }


        public static void SetSoftDeleteGlobalFilter(this ModelBuilder modelBuilder)
        {
            ApplyGlobalFilters<IOltEntityDeletable>(modelBuilder, p => p.DeletedOn == null);
        }


        /// <summary>
        /// <see href="https://davecallan.com/entity-framework-core-query-filters-multiple-entities"/>
        /// </summary>
        /// <example>
        /// <code>
        /// modelBuilder.HasQueryFilter(p => p.DeletedOn == null)
        /// </code>
        /// </example>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="expression"></param>
        public static void ApplyGlobalFilters<TEntity>(this ModelBuilder modelBuilder, Expression<Func<TEntity, bool>> expression)
        {

            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

#pragma warning disable S125

            EntitiesOfType<TEntity>(modelBuilder, builder =>
            {
                var clrType = builder.Metadata.ClrType;
                var tableName = builder.Metadata.GetDefaultTableName();

                if (tableName != null && clrType?.FullName != null)
                {
                    var discriminatorPropertyName = builder.Metadata.GetDiscriminatorPropertyName();
                    //TPH class?
                    if (discriminatorPropertyName != null)
                    {
                        if (clrType.BaseType != null && clrType.FullName.Equals(clrType.BaseType.FullName, StringComparison.OrdinalIgnoreCase))
                        {
                            //Console.WriteLine($"{tableName}: GetDiscriminatorProperty: {builder.Metadata.GetDiscriminatorPropertyName()} of type {builder.Metadata.ClrType.FullName}");
                            clrType = clrType.BaseType;
                            //Console.WriteLine($"{tableName}: {builder.Metadata.GetTableName()} not equal to {builder.Metadata.GetDefaultTableName()} of type {builder.Metadata.ClrType.FullName}");
                        }
                        else
                        {
                            clrType = null;  //Don't set the expression on the child TPH entity. This changed from EF Core 6 to EF Core 7
                        }
                    }

                    if (clrType != null)
                    {
                        var newParam = Expression.Parameter(clrType);
                        var newBody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
                        modelBuilder.Entity(clrType).HasQueryFilter(Expression.Lambda(newBody, newParam));
                    }

                }

            });


#pragma warning restore S125

        }

    }
}