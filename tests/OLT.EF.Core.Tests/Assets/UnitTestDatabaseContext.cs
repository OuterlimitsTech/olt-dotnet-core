using Microsoft.EntityFrameworkCore;
using OLT.Core;
using OLT.EF.Core.Tests.Assets.Entites;

namespace OLT.EF.Core.Tests.Assets
{
    public class UnitTestDatabaseContext : OltDbContext<UnitTestDatabaseContext>
    {
        public UnitTestDatabaseContext(DbContextOptions<UnitTestDatabaseContext> options) : base(options)
        {
        }

        public override string DefaultSchema => "UnitTest";
        public override bool DisableCascadeDeleteConvention => true;
        public override DefaultStringTypes DefaultStringType => DefaultStringTypes.NVarchar;
        public override bool ApplyGlobalDeleteFilter => true;

        public virtual DbSet<PersonEntity> People { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }
    }
}
