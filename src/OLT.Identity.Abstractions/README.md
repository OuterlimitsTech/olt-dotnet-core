[![CI](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-dotnet-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-dotnet-core)

## OLT Identity 

### OltIdentity

_Injectable Identity wrapper from IOltIdentity_


Abstract wrapper class that requires System.Security.Claims.ClaimsPrincipal.  This concept it to be injected 




#### Example using IHttpContextAccessor for a Asp.net Core web project with a Custom Claim
```csharp

public interface IAppIdentity : IOltIdentity
{
    string? MyCustomClaim { get; }
}

public class AppIdentity : OltIdentity, IAppIdentity
{
    private readonly IHttpContextAccessor _httpContext;
        

    public AppIdentity(IHttpContextAccessor httpContext)
    {
            
        _httpContext = httpContext;
    }

    public override ClaimsPrincipal Identity => _httpContext?.HttpContext?.User;

    public string? MyCustomClaim => GetClaims("MyCustomClaim").FirstOrDefault()?.Value; 
}


...

//Note: add to DI
services
    .AddScoped<IAppIdentity, AppIdentity>();

```

#### Example for a console app or batch service using a Role Enum
```csharp

// An enum used for a role using custom attribute "CodeAttribute" from OLT.Core.Attribute.Abstractions
public enum SecurityRoles 
{
    [Code("read-only")]
    [Description("View all records")]
    ReadOnly = 1000
}


public class BatchServiceIdentity : OltIdentity, IOltIdentity
{
    public BatchServiceIdentity()
    {
    }

    public const string SERVICE_NAME = "Batch Service";
    public override string NameId => "Unique Id boes into this claim";
    public override string Username => SERVICE_NAME;


    public override ClaimsPrincipal Identity
    {
        get
        {
            var roles = new List<string>();
            foreach (SecurityRoles role in Enum.GetValues(typeof(SecurityRoles)))
            {
                roles.Add(role.GetCodeEnum());
            }
            return new GenericPrincipal(new GenericIdentity(SERVICE_NAME), roles.ToArray());
        }
    }
}


public class MyExampleService : OltCoreService, IMyExampleService
{
    private readonly IOltIdentity _identity;

    public ReportManager(IOltIdentity identity)

    {
        _identity = identity;
    }

    public void ExampleMethod()
    {
        if (_identity.HasRole(SecurityRoles.ReadOnly)) 
        {
            //The Identity has the role
        }
                
        Debug.WriteLine(_identity.Username);
        Debug.WriteLine(_identity.NameId);
        Debug.WriteLine(_identity.FirstName);
        Debug.WriteLine(_identity.LastName);

    }

   
}


...

//Note: add to DI
services.AddScoped<IOltIdentity, BatchServiceIdentity>();


```

