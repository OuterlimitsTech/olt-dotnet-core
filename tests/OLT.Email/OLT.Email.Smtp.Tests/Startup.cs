using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace OLT.Email.Smtp.Tests
{

    public class Startup
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
            var configuration = hostBuilderContext.Configuration;

            var portConfigValue = configuration.GetValue<string>("SMTP_PORT") ?? Environment.GetEnvironmentVariable("SMTP_PORT");
            var portNumber = string.IsNullOrEmpty(portConfigValue) ? 587 : Convert.ToInt32(portConfigValue);
            var smtpConfig = new OltSmtpConfiguration
            {
                Server = configuration.GetValue<string>("SMTP_HOST") ?? Environment.GetEnvironmentVariable("SMTP_HOST"),
                Port = portNumber,
                Username = configuration.GetValue<string>("SMTP_USERNAME") ?? Environment.GetEnvironmentVariable("SMTP_USERNAME"),
                Password = configuration.GetValue<string>("SMTP_PASSWORD") ?? Environment.GetEnvironmentVariable("SMTP_PASSWORD"),
                DisableSsl = false,
            };

            services.Configure<OltSmtpConfiguration>(opt =>
            {
                opt.Server = smtpConfig.Server;
                opt.Port = smtpConfig.Port;
                opt.Username = smtpConfig.Username;
                opt.Password = smtpConfig.Password;
                opt.DisableSsl = smtpConfig.DisableSsl;
            });

            services.AddSingleton<IOltSmtpConfiguration>(opt => smtpConfig);
        }
    }
}
