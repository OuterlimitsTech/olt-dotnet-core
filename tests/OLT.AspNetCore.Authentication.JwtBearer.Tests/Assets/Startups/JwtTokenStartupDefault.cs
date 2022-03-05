using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;

namespace OLT.AspNetCore.Authentication.JwtBearer.Tests.Assets.Startups
{
    public class JwtTokenStartupDefault : BaseJwtTokenStartup
    {
        public JwtTokenStartupDefault(IConfiguration configuration) : base(configuration) { }

        public void ConfigureServices(IServiceCollection services)
        {
            base.DefaultServices(services);
            services.AddJwtBearer(JwtTokenTestExts.GetOptions(), opts =>
            {
                opts.Authority = JwtTokenTestExts.Authority;
                opts.Audience = JwtTokenTestExts.Audience;
            });
        }
    }
}
