using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OLT.Libraries.UnitTest.Assets.Email.SendGrid;

namespace OLT.Email.SendGrid.Tests
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
            var configuration = hostBuilderContext.Configuration;

            var configSection = configuration.GetSection("SendGrid");
            services.Configure<OltEmailConfigurationSendGrid>(configSection);

            int? envGroupId = configuration.GetValue<int>("SENDGRID_UNSUBSCRIBE_GROUP_ID") > 0 ? 
                configuration.GetValue<int>("SENDGRID_UNSUBSCRIBE_GROUP_ID") : 
                Environment.GetEnvironmentVariable("SENDGRID_UNSUBSCRIBE_GROUP_ID").ToInt();

            services.Configure<SendGridProductionConfiguration>(opt =>
            {
                opt.ApiKey = configuration.GetValue<string>("SENDGRID_TOKEN") ?? Environment.GetEnvironmentVariable("SENDGRID_TOKEN");
                opt.From = new OltEmailAddress(configuration.GetValue<string>("SMTP_FROM_ADDRESS") ?? Environment.GetEnvironmentVariable("SMTP_FROM_ADDRESS"), "OLT Unit Test");
                opt.TemplateIdJson = configuration.GetValue<string>("SENDGRID_TMPL_JSON") ?? Environment.GetEnvironmentVariable("SENDGRID_TMPL_JSON");
                opt.TemplateIdTag = configuration.GetValue<string>("SENDGRID_TMPL_TAG") ?? Environment.GetEnvironmentVariable("SENDGRID_TMPL_TAG");
                opt.ToEmail =  configuration.GetValue<string>("SMTP_TO_ADDRESS") ?? Environment.GetEnvironmentVariable("SMTP_TO_ADDRESS");
                opt.UnsubscribeGroupId = envGroupId;
                opt.Production = false;
                opt.DisableClickTracking = false;
                opt.RunNumber = configuration.GetValue<string>("GITHUB_RUN_NUMBER") ??  Environment.GetEnvironmentVariable("GITHUB_RUN_NUMBER") ??  "[No Run Number]";
                opt.TestWhitelist = new OltEmailConfigurationWhitelist
                {
                    Email = new List<string> { opt.ToEmail }
                };
            });
        }
    }
}
