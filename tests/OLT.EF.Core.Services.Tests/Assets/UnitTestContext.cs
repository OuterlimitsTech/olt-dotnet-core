﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System.Data.Common;
using System.Linq;

namespace OLT.EF.Core.Services.Tests.Assets
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

        private DatabaseFacade _database;

        public override DatabaseFacade Database
        {
            get
            {
                return _database ??= new MockDatabaseFacade(this);
            }
        }
    }

}
