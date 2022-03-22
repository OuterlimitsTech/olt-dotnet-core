using OLT.Core;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace OLT.Extensions.DependencyInjection.Tests.Assets
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

    public class TestIdentity : OltIdentity
    {

        public override ClaimsPrincipal Identity
        {
            get
            {
                var roles = new List<string>();
                return new GenericPrincipal(new GenericIdentity(nameof(TestIdentity)), roles.ToArray());
            }
        }

        public override string Username => nameof(TestIdentity);
        public override string UserPrincipalName => $"{nameof(TestIdentity)}@unittest.com";
        public override string Email => $"{nameof(TestIdentity)}@unittesting.com";

        public override bool HasRole(string claimName)
        {
            return true;
        }
    }
}
