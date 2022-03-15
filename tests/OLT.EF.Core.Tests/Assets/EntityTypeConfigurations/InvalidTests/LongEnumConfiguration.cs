using OLT.Core;
using OLT.EF.Core.Tests.Assets.Entites.Code;

namespace OLT.EF.Core.Tests.Assets.EntityTypeConfigurations.InvalidTests
{
    public enum LongValueTypes : long
    {
        LongValue = long.MaxValue,

    }

    public class LongEnumConfiguration : OltEntityTypeConfiguration<UserType, LongValueTypes>
    {

    }
}
