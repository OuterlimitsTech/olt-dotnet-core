using Microsoft.EntityFrameworkCore;
using OLT.Core;

namespace OLT.EF.Core.SeedHelpers.Csv.Tests.Assets
{
    public class UnitTestContext : OltDbContext<UnitTestContext>
    {
        public UnitTestContext(DbContextOptions<UnitTestContext> options) : base(options)
        {
        }

        public override string DefaultSchema => "UnitTest";
        public override bool DisableCascadeDeleteConvention => true;
        public override OltContextStringTypes DefaultStringType => OltContextStringTypes.Varchar;
        public override bool ApplyGlobalDeleteFilter => false;


        public virtual DbSet<PersonTypeCodeEntity> PersonTypeCodes { get; set; }
    }
}
