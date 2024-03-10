#nullable disable
using AspNetCore.Authentication.ApiKey;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OLT.Constants;
using OLT.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OLT.AspNetCore.Tests.Assets
{
    public class StartupWithAuth : Startup
    {
        public StartupWithAuth(IConfiguration configuration) : base(configuration)
        {
        }


        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services
                .AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
                .AddApiKeyInHeaderOrQueryParams(options =>
                {
                    options.Realm = "Unit Test";
                    options.KeyName = "X-API-Key";
                    options.Events = new ApiKeyEvents
                    {
                        OnValidateKey = context => {
                            if (context.ApiKey.Equals("XYZ"))
                            {
                                context.ValidationSucceeded(new List<System.Security.Claims.Claim> {  new System.Security.Claims.Claim(OltClaimTypes.Role, SecurityPermissions.UpdateData.GetCodeEnum() )});
                            }
                            else if (context.ApiKey.Equals("123"))
                            {
                                context.ValidationSucceeded(new List<System.Security.Claims.Claim> { new System.Security.Claims.Claim(OltClaimTypes.Role, SecurityPermissions.ReadOnly.GetCodeEnum()) });
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
