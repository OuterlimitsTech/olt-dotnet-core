using Microsoft.Extensions.Configuration;

namespace OLT.AspNetCore.Serilog.Tests
{
    public class StartupDefault : BaseStartup
    {
        public StartupDefault(IConfiguration configuration) : base(configuration) { }

    }
}
