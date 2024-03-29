﻿using Microsoft.EntityFrameworkCore;
using OLT.Core;
using OLT.EF.Core.SqlServer.Tests.Assests.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.EF.Core.SqlServer.Tests.Assests
{

    public class UnitTestSqlContext : OltSqlDbContext<UnitTestSqlContext>
    {
        public UnitTestSqlContext(DbContextOptions<UnitTestSqlContext> options) : base(options)
        {
        }

        public override string DefaultSchema => "UnitTest";
        public override bool DisableCascadeDeleteConvention => true;
        public override OltContextStringTypes DefaultStringType => OltContextStringTypes.Varchar;
        public override bool ApplyGlobalDeleteFilter => true;
        protected override int IdentitySeed => 1000;
        protected override int IdentityIncrement => 2;


        public int IdentitySeedValue => IdentitySeed;
        public int IdentityIncrementValue => IdentityIncrement;

        public virtual DbSet<PersonEntity> People { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }
    }
}
