using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OLT.Core.Rules.Tests.Assets.Context
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
