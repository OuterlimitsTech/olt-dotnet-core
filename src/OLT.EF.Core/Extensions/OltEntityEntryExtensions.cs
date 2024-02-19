using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OLT.Constants;

namespace OLT.Core
{
    public static class OltEntityEntryExtensions
    {
        #region [ Prep Save Methods ]

        public static void SetAuditFields(this EntityEntry entityEntry, string auditUser)
        {
            if (entityEntry.Entity is IOltEntityAudit createModel)
            {
                var utcNow = DateTimeOffset.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    createModel.CreateUser = auditUser;
                    createModel.CreateDate = utcNow;
                }

                createModel.ModifyUser = auditUser;
                createModel.ModifyDate = utcNow;

            }
        }

        public static void SetAbstractFields(this EntityEntry entityEntry)
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

        public static void CallTriggers(this EntityEntry entityEntry, IOltDbContext context)
        {

            if (entityEntry.State == EntityState.Added)
            {
                (entityEntry.Entity as IOltInsertingRecord)?.InsertingRecord(context, entityEntry);
            }

            if (entityEntry.State == EntityState.Modified)
            {
                (entityEntry.Entity as IOltUpdatingRecord)?.UpdatingRecord(context, entityEntry);
            }

        }

        public static void CheckNullableStringFields(this EntityEntry entityEntry, OltNullableStringUtilities utilities)
        {
            var nullableStringFields = utilities.GetNullableStringPropertyMetaData(entityEntry);

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
                    throw new OltException($"CheckNullableStringFields: {entityEntry.Entity.GetType().FullName} -> {nullableStringField.PropertyName}", ex);
                }
            }
        }


        #endregion
    }
}