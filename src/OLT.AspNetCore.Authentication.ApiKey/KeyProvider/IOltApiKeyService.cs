using System;
using System.Threading.Tasks;
using AspNetCore.Authentication.ApiKey;
using OLT.Core;

namespace OLT.AspNetCore.Authentication
{
    [Obsolete("Deprecating Library in 8.x")]
    public interface IOltApiKeyService : IOltInjectableScoped
    {
        Task<IApiKey> ValidateAsync(string key);
    }
}