using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace OLT.Core
{
    public class OltNullableStringUtilities
    {
#pragma warning disable S2743 // Static fields should not be used in generic types
#pragma warning disable IDE0044 // Add readonly modifier
        private static volatile object _entityMetatdataCacheSyncRoot;
        private static volatile Dictionary<RuntimeTypeHandle, List<OltNullableStringPropertyMetaData>> _entityMetatdataCache;
        private static volatile string _stringTypeName;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore S2743 // Static fields should not be used in generic types

#pragma warning disable S3963 // "static" fields should be initialized inline
        static OltNullableStringUtilities()
        {
            _entityMetatdataCacheSyncRoot = new object();
            _stringTypeName = nameof(String);
            _entityMetatdataCache = new Dictionary<RuntimeTypeHandle, List<OltNullableStringPropertyMetaData>>();
        }
#pragma warning restore S3963 // "static" fields should be initialized inline


        #region [ GetNullableStringPropertyMetaData ]

#pragma warning disable CA1822

        // Note - this looks in the thread-safe static cache to avoid the repetitive reflection.  Especially important since these things are
        // used in a VERY tight loop....
        public List<OltNullableStringPropertyMetaData> GetNullableStringPropertyMetaData(EntityEntry entry)
        {
            var type = entry.Entity.GetType();
            var typeHandle = type.TypeHandle;

            // Fast return if we did this already.....
            List<OltNullableStringPropertyMetaData>? existing;
            if (_entityMetatdataCache.TryGetValue(typeHandle, out existing))
            {
                return existing;
            }

            List<OltNullableStringPropertyMetaData> result = new List<OltNullableStringPropertyMetaData>();

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

                var info = new OltNullableStringPropertyMetaData
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