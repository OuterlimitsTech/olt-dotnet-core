using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;

namespace OLT.AspNetCore.Serilog.Tests.Assets.Startups
{
    public class StartupDefault : BaseStartup
    {
        public StartupDefault(IConfiguration configuration) : base(configuration) { }

        public void ConfigureServices(IServiceCollection services)
        {
            base.DefaultServices(services);
            
        }
    }
}
