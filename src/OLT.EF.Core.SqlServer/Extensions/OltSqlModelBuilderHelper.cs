using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Core
{
    public static class OltSqlModelBuilderHelper
    {
        public static void SetIdentityColumns(ModelBuilder modelBuilder, int identitySeed, int identityIncrement)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

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
}
