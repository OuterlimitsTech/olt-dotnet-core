[![CI](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-dotnet-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-dotnet-core)

## General Model Classes and Interfaces.  Includes Constant Classes

### OltClaimTypes

_List of registered claims from different sources_

https://datatracker.ietf.org/doc/html/rfc7519#section-4
http://openid.net/specs/openid-connect-core-1_0.html#IDToken
https://github.com/openiddict/openiddict-core/blob/dev/src/OpenIddict.Abstractions/OpenIddictConstants.cs
https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/23808d5c7b11c3e0e9f202e48129c054e2b4f7ab/src/Microsoft.IdentityModel.JsonWebTokens/JwtRegisteredClaimNames.cs


### Models

_I use naming convention "Json" on the end of a class to indicate it's exposed from an API controller (i.e., MyDataJson)_

#### OltPersonName & IOltPersonName

#### OltAuthenticatedUserJson & OltAuthenticatedUserJwtTokenJson

#### OltFileBase64 & IOltFileBase64 - Represents a file encoded as a base64 string

#### IOltPaged - Paged resultset

#### OltPagingParams & IOltPagingParams - Paged Parameter Class used by an ASP.Net Controller Parameter

#### OltPagedJson & IOltPagedJson - Paged Json Model

#### OltPagedSearchJson - Paged Resultset with the search criteria class included


```csharp
[Route("api/my-search-example")]
[RequirePermission(SecurityPermissions.ReadOnly)]
public class SearchController : BaseApiController
{
    private readonly IMyRepoService _service;

    public SearchController(
        IMyRepoService service,
        IHelperService helperService) : base(helperService)
    {
        _service = service;
    }

    [HttpPost, Route("")]
    public async Task<ActionResult<SearchKeyJson>> Search([FromBody] SearchCriteriaJson criteria)
    {
        return Ok(await _service.SearchAsync(new MySearcher(criteria)));
    }

    [HttpGet, Route("{searchKey}")]
    public async Task<ActionResult<OltPagedSearchJson<SearchResultJson, SearchCriteriaJson>>> GetSearch(string searchKey, [FromQuery] OltPagingParams @params)
    {
        return Ok(await _service.GetPagedAsync(searchKey, @params));
    }
}
```



### OltIdentity

_Injectable Identity wrapper from IOltIdentity_


Abstract wrapper class that requires System.Security.Claims.ClaimsPrincipal.


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


