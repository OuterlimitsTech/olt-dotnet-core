using AspNetCore.Authentication.ApiKey;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OLT.AspNetCore.Tests.Assets
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected IConfiguration Configuration { get; }


        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddOltAspNetCore();
            services.AddRouting();            
        }


        public virtual void Configure(IApplicationBuilder app)
        {            
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

    }


    public class StartupWithAuth : Startup
    {
        public StartupWithAuth(IConfiguration configuration) : base(configuration)
        {
        }


        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
                .AddApiKeyInHeaderOrQueryParams(options =>
                {
                    options.Realm = "Unit Test";
                    options.KeyName = "X-API-Key";
                    options.Events = new ApiKeyEvents
                    {
                        OnValidateKey = context => {
                            if (context.ApiKey.Equals("XYZ"))
                            {
                                context.ValidationSucceeded(new List<Claim> {  new Claim(ClaimTypes.Role, SecurityPermissions.UpdateData.GetCodeEnum() )});
                            }
                            else
                            {
                                context.ValidationFailed();
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
            
        }


        public override void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseAuthentication();            
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        } 

    }
}
