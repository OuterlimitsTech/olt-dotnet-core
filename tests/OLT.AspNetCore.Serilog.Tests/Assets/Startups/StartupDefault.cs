using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OLT.AspNetCore.Serilog.Tests.Assets.Startups
{
    public class StartupDefault : BaseStartup
    {
        public StartupDefault(IConfiguration configuration) : base(configuration) { }
      
    }
}
