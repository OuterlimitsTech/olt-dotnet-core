using System.Security.Claims;
using System.Security.Principal;

namespace OLT.Core.Model.Abstractions.Tests;

public class TestIdentity : OltIdentity
{
    private readonly ClaimsPrincipal? _identity;

    public TestIdentity()
    {

    }

    public TestIdentity(GenericIdentity identity)
    {
        _identity = new GenericPrincipal(identity, null);
    }

    public TestIdentity(ClaimsPrincipal identity)
    {
        _identity = identity;
    }

    public override ClaimsPrincipal? Identity => _identity;

    public override bool HasRole<TRoleEnum>(params TRoleEnum[] roles)
    {
        return roles?.Any(role => HasRole(role.GetCodeEnum())) == true;
    }
}
