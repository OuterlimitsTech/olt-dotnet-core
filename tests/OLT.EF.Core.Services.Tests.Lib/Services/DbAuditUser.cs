using OLT.Core;

namespace OLT.EF.Core.Services.Tests.Lib.Services;

public class DbAuditUser : IOltDbAuditUser
{
    private readonly string _username;

    public DbAuditUser()
    {
        _username = Faker.Internet.UserName();
    }

    public string GetDbUsername()
    {
        return _username;
    }
}
