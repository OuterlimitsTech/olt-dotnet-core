using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace OLT.AspNetCore.Serilog.Tests.Assets
{
    public static class TestHostBuilder
    {

        public static IWebHostBuilder WebHostBuilder<T>() where T : class
        {
            var webBuilder = new WebHostBuilder();
            webBuilder
                .UseSerilog()
                .ConfigureAppConfiguration(builder =>
                {
                    builder
                        .SetBasePath(AppContext.BaseDirectory)
                        .AddUserSecrets<T>()
                        .AddJsonFile("appsettings.json", true, false)
                        .AddEnvironmentVariables();
                })
                .UseStartup<T>();

            return webBuilder;
        }
    }
}
