using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace OLT.Core
{
    public sealed class OltNullableStringPropertyMetaData
    {
        public EntityEntry? EntityEntry { get; set; }
        public string? PropertyName { get; set; }
        public MethodInfo? Getter { get; set; }
        public MethodInfo? Setter { get; set; }


        // Note - we use the GetGetter approach because EF may be a detached poco, dynamic proxy, or dynamic object.  
        // Simply using GetValue off PropertyInfo on a dynamic object will fail (same is true in EF Core).
        public string? GetValue(EntityEntry source)
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

            this.Setter?.Invoke(source.Entity, new object?[] { null });
        }
    }
}