using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OLT.Extensions.Caching.Tests.Assets;

namespace OLT.Extensions.Caching.Tests
{
    internal class Startup
    {

        public void ConfigureHost(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureHostConfiguration(builder =>
            {
                builder
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", true)
                    .AddEnvironmentVariables()
                    .AddUserSecrets<Startup>();
            });
        }

        public virtual void ConfigureServices(IServiceCollection services, HostBuilderContext hostBuilderContext)
        {
            //SendGrid uses Newtsoft to Convert, but doesn't give a way to change the resolver, so you have to do it globally. YUCK!!!
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var configuration = hostBuilderContext.Configuration;

            services.AddLogging(config => config.AddConsole());
            

            services.Configure<CacheConfiguration>(opt =>
            {
                opt.RedisCacheConnectionString = configuration.GetValue<string>("REDIS_CACHE") ?? Environment.GetEnvironmentVariable("REDIS_CACHE");
            });
        }
    }
}
