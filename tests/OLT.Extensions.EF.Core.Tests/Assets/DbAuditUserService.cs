using OLT.Core;

namespace OLT.Extensions.EF.Core.Tests.Assets
{
    public class EmtpyDbAuditUserService : IOltDbAuditUser
    {
        public string GetDbUsername()
        {
            return null;
        }
    }

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
