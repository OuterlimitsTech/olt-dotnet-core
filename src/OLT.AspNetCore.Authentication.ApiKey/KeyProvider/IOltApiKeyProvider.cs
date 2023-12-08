using AspNetCore.Authentication.ApiKey;
using OLT.Core;
using System;

namespace OLT.AspNetCore.Authentication
{
    [Obsolete("Deprecating Library in 8.x")]
    public interface IOltApiKeyProvider : IApiKeyProvider, IOltInjectableScoped
    {
        
    }
}