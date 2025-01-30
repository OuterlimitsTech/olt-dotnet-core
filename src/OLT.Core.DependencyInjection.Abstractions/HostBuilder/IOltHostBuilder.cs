using Microsoft.Extensions.DependencyInjection;

namespace OLT.Core;

public interface IOltHostBuilder
{
    /// <summary>
    /// Gets a collection of services for the application to compose. This is useful for adding user provided or framework provided services.
    /// </summary>
    IServiceCollection Services { get; }     
}