using Microsoft.Extensions.DependencyInjection;

namespace OLT.Core;

public class OltHostBuilder : IOltHostBuilder
{

    public OltHostBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }

}


