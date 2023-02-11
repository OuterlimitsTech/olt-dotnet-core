using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace OLT.AspNetCore.Versioning.Tests.Assets
{
    public static class TestHelper
    {
        public static IWebHostBuilder WebHostBuilder<T>() where T : class
        {
            var webBuilder = new WebHostBuilder();
            webBuilder
                .ConfigureAppConfiguration(builder =>
                {
                    builder
                        .SetBasePath(AppContext.BaseDirectory)
                        //.AddUserSecrets<T>()
                        //.AddJsonFile("appsettings.json", true, false)
                        .AddEnvironmentVariables();
                })
                .UseWebRoot(AppContext.BaseDirectory)
                .UseStartup<T>();

            return webBuilder;
        }
    }
}
