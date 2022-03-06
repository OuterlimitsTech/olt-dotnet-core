using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;

namespace OLT.AspNetCore.Authentication.JwtBearer.Tests.Assets.Startups
{
    public class WithOptionsStartupTest : BaseJwtTokenStartup
    {   
        public WithOptionsStartupTest(IConfiguration configuration) : base(configuration)
        {
            
        }
    
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddJwtBearer(JwtTokenTestExts.GetOptions(), opts =>
            {
                opts.Authority = JwtTokenTestExts.Authority;
                opts.Audience = JwtTokenTestExts.Audience;
            });
        }
    }

    public class WithoutOptionsStartupTest : BaseJwtTokenStartup
    {
        public WithoutOptionsStartupTest(IConfiguration configuration) : base(configuration)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddJwtBearer(JwtTokenTestExts.GetOptions());
        }
    }
}
