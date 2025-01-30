using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OLT.Core;

public static class OltSqlModelBuilderExtensions
{
    /// <summary>
    /// Sets the <see cref="Microsoft.EntityFrameworkCore.SqlServerPropertyBuilderExtensions.UseIdentityColumn{TProperty}(Microsoft.EntityFrameworkCore.Metadata.Builders.PropertyBuilder{TProperty}, int, int)"/> for interfaces <see cref="IOltEntityId"/>
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="identitySeed"></param>
    /// <param name="identityIncrement"></param>
    public static void SetIdentityColumns(this ModelBuilder modelBuilder, int identitySeed, int identityIncrement)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        OltModelBuilderExtensions.EntitiesOfType<IOltEntityId>(modelBuilder, builder =>
        {
            var prop = builder.Property<int>(nameof(IOltEntityId.Id));
            if (prop.Metadata.ValueGenerated == ValueGenerated.OnAdd && !prop.Metadata.GetIdentitySeed().HasValue)
            {
                prop.UseIdentityColumn(identitySeed, identityIncrement);
            }

#pragma warning disable S125
            //Console.WriteLine($"{builder.Metadata.GetTableName()} of type {builder.Metadata.ClrType.FullName} -> GetIdentitySeed: {prop.Metadata.GetIdentitySeed()} -> IdentitySeed: {IdentitySeed}{Environment.NewLine}");
#pragma warning restore S125
        });
    }
}
