using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations.Schema;

namespace OLT.Core
{
    public static class OltModelBuilderHelper
    {
        /// <summary>
        /// Sets the DB to TableNameId for entities of <see cref="IOltEntityId"/>, unless the <see cref="ColumnAttribute"/> is used
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void EntityIdColumnName(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            OltModelBuilderExtensions.EntitiesOfType<IOltEntityId>(modelBuilder, builder =>
            {
                var tableName = builder.Metadata.GetTableName();
                if (tableName != null)
                {
                    var prop = builder.Property<int>(nameof(IOltEntityId.Id));
                    var eval = prop.Metadata.GetColumnName(StoreObjectIdentifier.Table(tableName, builder.Metadata.GetSchema()));
                    if (eval != null && eval.Equals("Id", StringComparison.OrdinalIgnoreCase))
                    {
                        var columnName = $"{builder.Metadata.GetTableName()}Id";
                        builder.Property<int>(nameof(IOltEntityId.Id)).HasColumnName(columnName);
                    }
                }
        
            });
        }

        /// <summary>
        /// Returns all FKs where <see cref="DeleteBehavior.Cascade"/>
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static IEnumerable<IMutableForeignKey> GetAllCascadeDelete(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            return modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
        }


        /// <summary>
        /// Restricts all FKs to <see cref="DeleteBehavior.Restrict"/>
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void RestrictDeleteBehavior(ModelBuilder modelBuilder)
        {
            var cascadeFKs = GetAllCascadeDelete(modelBuilder);
            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        /// <summary>
        /// Returns all Unicode <see cref="string"/> columns
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static IEnumerable<IMutableProperty> GetAllUnicodeProperties(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            return modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(string)).Where(property => property.IsUnicode().GetValueOrDefault(true));
        }

        /// <summary>
        /// Disable Unicode for all <see cref="string"/> columns
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static void DisableUnicodeProperties(ModelBuilder modelBuilder)
        {
            var properties = GetAllUnicodeProperties(modelBuilder);
            foreach (var property in properties)
            {
                property.SetIsUnicode(false);
            }
        }
    }
}