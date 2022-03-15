using Microsoft.EntityFrameworkCore;
using OLT.Core;
using OLT.EF.Core.Tests.Assets.Entites;

namespace OLT.EF.Core.Tests.Assets
{
    public class UnitTestAlternateContext : OltDbContext<UnitTestAlternateContext>
    {
        public UnitTestAlternateContext(DbContextOptions<UnitTestAlternateContext> options) : base(options)
        {
        }

        public override string DefaultSchema => null;
        public override bool DisableCascadeDeleteConvention => false;
        public override OltContextStringTypes DefaultStringType => OltContextStringTypes.NVarchar;
        public override bool ApplyGlobalDeleteFilter => false;

        public virtual DbSet<PersonEntity> People { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }
    }
}
