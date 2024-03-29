﻿using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace OLT.AspNetCore.Authentication.JwtBearer.Tests.Assets
{
    public static class TestHostBuilder
    {

        public static IWebHostBuilder WebHostBuilder<T>() where T : class
        {
            var webBuilder = new WebHostBuilder();
            webBuilder
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
