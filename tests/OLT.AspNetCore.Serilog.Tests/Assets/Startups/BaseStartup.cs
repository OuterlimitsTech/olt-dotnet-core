using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.AspNetCore.Serilog.Tests.Assets.Startups
{
    public abstract class BaseStartup
    {
        protected BaseStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app)
        {
            
        }

        public void DefaultServices(IServiceCollection services)
        {
            //Do Nothing
        }
    }
}
