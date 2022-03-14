using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OLT.Core;
using OLT.EF.Core.Tests.Assets.Entites;
using OLT.EF.Core.Tests.Assets.Entites.Code;
using System;
using System.Data.Common;

namespace OLT.EF.Core.Tests.Assets
{
    public class UnitTestContext : OltDbContext<UnitTestContext>
    {
        public UnitTestContext(DbContextOptions<UnitTestContext> options) : base(options)
        {
        }

        public override string DefaultSchema => "UnitTest";
        public override bool DisableCascadeDeleteConvention => true;
        public override OltContextStringTypes DefaultStringType => OltContextStringTypes.Varchar;
        public override bool ApplyGlobalDeleteFilter => true;

        public virtual DbSet<PersonEntity> People { get; set; }
        public virtual DbSet<AddressEntity> Addresses { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<NoIdEntity> NoIdentifiers { get; set; }
        public virtual DbSet<StatusTypeCodeTableEntity> StatusTypes { get; set; }

    }

}
