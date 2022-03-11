using System.Threading.Tasks;
using AspNetCore.Authentication.ApiKey;
using OLT.Core;

namespace OLT.AspNetCore.Authentication
{
    public interface IOltApiKeyService : IOltInjectableScoped
    {
        Task<IApiKey> ValidateAsync(string key);
    }
}