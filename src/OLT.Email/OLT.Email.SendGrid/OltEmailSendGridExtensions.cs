using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;

namespace OLT.Email.SendGrid
{

    public static class OltEmailSendGridExtensions
    {
        public static OltSendGridTemplateArgs BuildArgs<T>(this OltEmailConfigurationSendGrid configuration, T template)
            where T : IOltEmailTemplate
        {
            var args = new OltSendGridTemplateArgs()
                .WithFromEmail(configuration.From)
                .WithWhitelist(configuration.TestWhitelist)
                .WithApiKey(configuration.ApiKey)
                .WithTemplate(template)
                .EnableProductionEnvironment(configuration.Production);

            if (configuration.DisableClickTracking)
            {
                args.WithoutClickTracking();
            }

            return args;
        }


    }
}
