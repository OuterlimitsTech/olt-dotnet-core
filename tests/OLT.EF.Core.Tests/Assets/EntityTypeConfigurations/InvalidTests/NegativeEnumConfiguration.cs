using OLT.Core;
using OLT.EF.Core.Tests.Assets.Entites.Code;

namespace OLT.EF.Core.Tests.Assets.EntityTypeConfigurations.InvalidTests
{
    public enum InvalidTypes
    {
        NegativeValue = -100,

    }

    public class NegativeEnumConfiguration : OltEntityTypeConfiguration<UserType, InvalidTypes>
    {

    }
}
