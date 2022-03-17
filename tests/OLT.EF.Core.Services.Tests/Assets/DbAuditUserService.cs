using Microsoft.Extensions.Logging;
using OLT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.EF.Core.Tests.Assets
{

    public class DbAuditUserService : IOltDbAuditUser
    {
        private readonly string _username;

        public DbAuditUserService()
        {
            _username = Faker.Internet.UserName();
        }

        public string GetDbUsername()
        {
            return _username;
        }
    }
}
