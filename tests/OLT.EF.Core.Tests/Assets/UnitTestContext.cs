using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OLT.Core;
using OLT.EF.Core.Tests.Assets.Entites;
using OLT.EF.Core.Tests.Assets.Entites.Code;
using System;
using System.Data.Common;
using System.Linq;

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
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<StatusTypeCodeTableEntity> StatusTypes { get; set; }
        public virtual DbSet<PersonTypeCodeTableEntity> PersonTypes { get; set; }

        public virtual DbSet<NoIdEntity> NoIdentifiers { get; set; }
        public virtual DbSet<NoStringEntity> NoStringEntities { get; set; }
        public virtual DbSet<EmptyExceptionStringEntity> EmptyExceptionStringEntities { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonEntity>().Property(p => p.NameFirst).IsRequired().HasMaxLength(50);
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// This is to force the memory DB to throw exceptions as the provider does not!
        /// </summary>
        /// <exception cref="DbUpdateException"></exception>
        private void CheckValuesException()
        {
            var entries = this.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged).ToList();
            foreach (var entry in entries)
            {
                foreach (var prop in entry.CurrentValues.Properties)
                {
                    var val = prop.PropertyInfo.GetValue(entry.Entity);
                    if (val?.ToString().Length > prop.GetMaxLength())
                    {
                        throw new DbUpdateException("UnitTest Overflow", entries);
                    }
                    else if (val == null && !prop.IsColumnNullable())
                    {
                        throw new Exception("UnitTest Null");
                    }
                }
            }
        }
        protected override void PrepareToSave()
        {            
            base.PrepareToSave();
            CheckValuesException();
        }
    }

}
