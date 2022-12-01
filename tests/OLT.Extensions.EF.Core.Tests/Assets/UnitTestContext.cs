using Microsoft.EntityFrameworkCore;
using OLT.Core;
using OLT.Extensions.EF.Core.Tests.Assets.Entites;
using System;
using System.Linq;

namespace OLT.Extensions.EF.Core.Tests.Assets
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

        public virtual DbSet<UserEntity> Users { get; set; }


        ///// <summary>
        ///// This is to force the memory DB to throw exceptions as the provider does not!
        ///// </summary>
        ///// <exception cref="DbUpdateException"></exception>
        //private void CheckValuesException()
        //{
        //    var entries = ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged).ToList();
        //    foreach (var entry in entries)
        //    {
        //        foreach (var prop in entry.CurrentValues.Properties)
        //        {
        //            var val = prop.PropertyInfo.GetValue(entry.Entity);
        //            if (val?.ToString().Length > prop.GetMaxLength())
        //            {
        //                throw new DbUpdateException("UnitTest Overflow", entries);
        //            }
        //            else if (val == null && !prop.IsColumnNullable())
        //            {
        //                throw new Exception("UnitTest Null");
        //            }
        //        }
        //    }
        //}
        //protected override void PrepareToSave()
        //{
        //    base.PrepareToSave();
        //    CheckValuesException();
        //}
    }

}
