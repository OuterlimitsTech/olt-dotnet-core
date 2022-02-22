using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OLT.Email.Tests
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

            services.Configure<OltEmailConfiguration>(configuration.GetSection("EmailConfig"));
            services.Configure<OltSmtpConfiguration>(configuration.GetSection("SmtpEmailConfig"));

            //var portConfigValue = configuration.GetValue<string>("SMTP_PORT") ?? Environment.GetEnvironmentVariable("SMTP_PORT");
            //short? portNumber = string.IsNullOrEmpty(portConfigValue) ? null : Convert.ToInt16(portConfigValue);
            //var smtpConfig = new Smtpser
            //{
            //    Host = configuration.GetValue<string>("SMTP_HOST") ?? Environment.GetEnvironmentVariable("SMTP_HOST"),
            //    Port = portNumber,
            //    Username = configuration.GetValue<string>("SMTP_USERNAME") ?? Environment.GetEnvironmentVariable("SMTP_USERNAME"),
            //    Password = configuration.GetValue<string>("SMTP_PASSWORD") ?? Environment.GetEnvironmentVariable("SMTP_PASSWORD"),
            //    DisableSsl = false,
            //};

            //services.Configure<SmtpServer>(opt =>
            //{
            //    opt.Host = smtpConfig.Host;
            //    opt.Port = smtpConfig.Port;
            //    opt.Username = smtpConfig.Username;
            //    opt.Password = smtpConfig.Password;
            //    opt.DisableSsl = smtpConfig.DisableSsl;
            //});

        }
    }
}
